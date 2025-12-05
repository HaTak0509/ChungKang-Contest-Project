using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private float sinkAmount = 0.2f;  // 얼마나 내려갈지
    [SerializeField] private float sinkSpeed = 10f;    // 내려가는 속도
    [SerializeField] private Sprite pressedSprite;     // 눌린 스프라이트

    private Vector3 originalPosition;
    private bool isPressed = false;
    private Transform playerParent;

    void Start()
    {
        originalPosition = transform.position;  // 원래 위치 저장
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어를 발판 자식으로 만들어 따라 내려가게 함 (깔끔한 움직임)
            playerParent = collision.transform;
            playerParent.SetParent(transform);
            isPressed = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어 부모 해제
            playerParent.SetParent(null);
            playerParent = null;
            isPressed = false;
        }
    }

    void Update()
    {
        // Lerp로 부드럽게 위치 이동
        Vector3 targetPos = isPressed ? originalPosition + Vector3.down * sinkAmount : originalPosition;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * sinkSpeed);
    }
}
