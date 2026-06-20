using UnityEngine;

// 이 스크립트를 오브젝트에 붙이는 순간 BoxCollider2D가 자동으로 추가됩니다.
[RequireComponent(typeof(BoxCollider2D))]
public class Ground : MonoBehaviour
{
    private BoxCollider2D _collider;

    private void Awake()
    {
        // 1. 내 몸에 붙어있는 BoxCollider2D를 가져옵니다.
        _collider = GetComponent<BoxCollider2D>();

        // 2. 바닥은 절대 뚫리면 안 되는 물리적인 벽이므로, 
        // 혹시라도 에디터에서 실수로 Is Trigger가 체크되어 있다면 코드로 강제 해제합니다.
        if (_collider != null)
        {
            _collider.isTrigger = false;
        }

        // 3. 만약 오브젝트의 태그가 기본값(Untagged)이라면 Ground로 바꿔줍니다.
        // (주의: 유니티 에디터 상단 Tags 메뉴에 "Ground"라는 태그가 미리 만들어져 있어야 합니다!)
        if (gameObject.CompareTag("Untagged"))
        {
            gameObject.tag = "Ground";
        }
    }
}