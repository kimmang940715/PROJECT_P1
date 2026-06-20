using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("스폰 설정")]
    public GameObject meteorPrefab; // 생성할 유성 원본 (프리팹)
    public float spawnInterval = 1.5f; // 몇 초마다 생성할 것인지

    [Header("스폰 위치 구역")]
    public float spawnMinX = -8f; // 스폰 구역 왼쪽 끝 X 좌표
    public float spawnMaxX = 8f;  // 스폰 구역 오른쪽 끝 X 좌표
    public float spawnY = 6f;     // 스폰 구역 높이 (화면 위쪽 보이지 않는 곳)

    private void Start()
    {
        // 게임이 시작되면 유성 생성 루틴(코루틴)을 실행합니다.
        StartCoroutine(SpawnMeteorRoutine());
    }

    private IEnumerator SpawnMeteorRoutine()
    {
        // 게임 시작 후 플레이어가 준비할 수 있게 1초 정도 대기
        yield return new WaitForSeconds(1.0f);

        // 무한 루프로 계속 돌립니다.
        while (true)
        {
            // 만약 게임 매니저가 존재하고, 게임이 오버(또는 클리어)된 상태라면 루프를 종료(스폰 중지)합니다.
            if (GameManager.Instance != null && GameManager.Instance.isGameOver)
            {
                Debug.Log("게임 종료 확인됨: 유성 스폰을 중지합니다.");
                break; // while 루프 탈출
            }

            // 랜덤한 X 위치 뽑기
            float randomX = Random.Range(spawnMinX, spawnMaxX);
            Vector2 spawnPosition = new Vector2(randomX, spawnY);

            // 유성 생성! (Quaternion.identity는 회전 없이 그대로 생성한다는 뜻입니다)
            if (meteorPrefab != null)
            {
                Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("SpawnManager에 유성(Meteor) 프리팹이 연결되어 있지 않습니다!");
            }

            // 설정한 시간(spawnInterval)만큼 쉬었다가 다음 루프로 넘어갑니다.
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}