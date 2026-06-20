using System;
using System.Data;
using UnityEngine;

// --- DB 종류를 선택하기 위한 열거형 ---
public enum DatabaseType
{
    MySQL,
    MSSQL,
    Oracle
}

// --- 공통 DB 기능 인터페이스 ---
public interface IDatabaseService
{
    bool Connect(string connectionString);
    void Disconnect();
    void SaveScore(string playerName, int score);
}

// --- 네트워크 및 DB 관리를 총괄하는 매니저 ---
public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _instance;

    // 프로퍼티를 통해 외부에서 접근할 때 체크 및 자동 생성 수행
    public static NetworkManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 1. 씬에 이미 NetworkManager가 배치되어 있는지 확인 (Unity 6 권장 함수 사용)
                _instance = FindAnyObjectByType<NetworkManager>();

                // 2. 없다면 새로 생성
                if (_instance == null)
                {
                    GameObject go = new GameObject("NetworkManager_AutoCreated");
                    _instance = go.AddComponent<NetworkManager>();

                    // 씬이 바뀌어도 파괴되지 않도록 설정
                    DontDestroyOnLoad(go);
                    Debug.Log("씬에 NetworkManager가 없어 코드가 자동으로 생성했습니다.");
                }
            }
            return _instance;
        }
    }

    [Header("Database Settings")]
    public DatabaseType currentDbType = DatabaseType.MySQL;

    [Tooltip("실제 서비스 시 이곳에 비밀번호를 직접 적는 것은 매우 위험합니다!")]
    public string connectionString = "Server=127.0.0.1;Database=GameDB;User ID=root;Password=1234;";

    private IDatabaseService dbService;

    private void Awake()
    {
        // 중복 방지 및 초기화 로직
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDatabase();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // 1. 선택한 DB 타입에 따라 적절한 서비스 객체 생성
    private void InitializeDatabase()
    {
        switch (currentDbType)
        {
            case DatabaseType.MySQL:
                dbService = new MySQLService();
                break;
            case DatabaseType.MSSQL:
                dbService = new MSSQLService();
                break;
            case DatabaseType.Oracle:
                dbService = new OracleService();
                break;
        }

        // 연결 시도
        if (dbService.Connect(connectionString))
        {
            Debug.Log($"[{currentDbType}] DB 연결 성공!");
        }
        else
        {
            Debug.LogError($"[{currentDbType}] DB 연결 실패. Connection String이나 플러그인을 확인하세요.");
        }
    }

    // 2. 게임 매니저 등 외부에서 이 함수를 호출하여 점수 저장
    public void SaveGameData(int finalScore)
    {
        if (dbService != null)
        {
            // 임시 플레이어 이름 (나중엔 로그인 정보 연동)
            string playerName = "Player_" + UnityEngine.Random.Range(1000, 9999);
            dbService.SaveScore(playerName, finalScore);
        }
    }

    private void OnApplicationQuit()
    {
        if (dbService != null)
        {
            dbService.Disconnect();
            Debug.Log("게임 종료: DB 연결 해제");
        }
    }
}

// =================================================================================
// 아래부터는 각 DB별 실제 연결 구현체입니다. 
// 사용할 DB의 .dll 파일이 Plugins 폴더에 있어야 에러가 나지 않습니다.
// (에러가 난다면 안 쓰는 DB 코드는 주석 처리하세요)
// =================================================================================

public class MySQLService : IDatabaseService
{
    // MySql.Data.MySqlClient.MySqlConnection 이 필요합니다.
    private IDbConnection dbConnection;

    public bool Connect(string connectionString)
    {
        try
        {
            // Reflection을 사용하여 dll이 없을 때 발생하는 컴파일 에러를 우회합니다. (실제 환경에선 직접 선언 권장)
            Type type = Type.GetType("MySql.Data.MySqlClient.MySqlConnection, MySql.Data");
            if (type == null)
            {
                Debug.LogError("MySQL dll(MySql.Data.dll)을 찾을 수 없습니다.");
                return false;
            }

            dbConnection = (IDbConnection)Activator.CreateInstance(type, connectionString);
            dbConnection.Open();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("MySQL Connect Error: " + e.Message);
            return false;
        }
    }

    public void SaveScore(string playerName, int score)
    {
        if (dbConnection != null && dbConnection.State == ConnectionState.Open)
        {
            // SQL Injection 방지를 위해 파라미터 사용 필수
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // 간단한 예시 쿼리입니다. 실제 테이블 구조에 맞게 수정해야 합니다.
                dbCmd.CommandText = $"INSERT INTO user_scores (username, score) VALUES ('{playerName}', {score})";
                dbCmd.ExecuteNonQuery();
                Debug.Log($"MySQL에 점수 저장 완료: {playerName} - {score}");
            }
        }
    }

    public void Disconnect()
    {
        if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
        {
            dbConnection.Close();
            dbConnection.Dispose();
        }
    }
}

public class MSSQLService : IDatabaseService
{
    private IDbConnection dbConnection;

    public bool Connect(string connectionString)
    {
        try
        {
            // System.Data.SqlClient.SqlConnection
            Type type = Type.GetType("System.Data.SqlClient.SqlConnection, System.Data");
            if (type == null) { Debug.LogError("MSSQL dll을 찾을 수 없습니다."); return false; }

            dbConnection = (IDbConnection)Activator.CreateInstance(type, connectionString);
            dbConnection.Open();
            return true;
        }
        catch (Exception e) { Debug.LogError("MSSQL Error: " + e.Message); return false; }
    }

    public void SaveScore(string playerName, int score) { /* 위 MySQL과 로직 동일 */ }
    public void Disconnect() { if (dbConnection != null) dbConnection.Close(); }
}

public class OracleService : IDatabaseService
{
    private IDbConnection dbConnection;

    public bool Connect(string connectionString)
    {
        try
        {
            // Oracle.ManagedDataAccess.Client.OracleConnection
            Type type = Type.GetType("Oracle.ManagedDataAccess.Client.OracleConnection, Oracle.ManagedDataAccess");
            if (type == null) { Debug.LogError("Oracle dll을 찾을 수 없습니다."); return false; }

            dbConnection = (IDbConnection)Activator.CreateInstance(type, connectionString);
            dbConnection.Open();
            return true;
        }
        catch (Exception e) { Debug.LogError("Oracle Error: " + e.Message); return false; }
    }

    public void SaveScore(string playerName, int score) { /* 위 MySQL과 로직 동일 */ }
    public void Disconnect() { if (dbConnection != null) dbConnection.Close(); }
}