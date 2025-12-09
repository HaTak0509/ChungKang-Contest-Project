using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Door targetDoor;
    [SerializeField] private Bridge targetBridge;

    [SerializeField] private float sinkAmount = 0.2f;  // 얼마나 내려갈지
    [SerializeField] private float sinkSpeed = 10f;    // 내려가는 속도
    [SerializeField] private Sprite pressedSprite;

    private Vector3 originalPosition;
    private bool isPressed = false;
    private Transform player;

    void Start()
    {
        // 원래 위치 저장
        originalPosition = transform.position;  
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform;
            isPressed = true;
            if (targetDoor != null) targetDoor.OpenDoor();
            if (targetBridge != null) targetBridge.OpenBridge();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = null;
            isPressed = false;
            if (targetDoor != null) targetDoor.CloseDoor();
            if (targetBridge != null) targetBridge.CloseBridge();
        }
    }
    void Update()
    {
        Vector3 prevPos = transform.position;

        Vector3 targetPos = isPressed ? originalPosition + Vector3.down * sinkAmount : originalPosition;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * sinkSpeed);

        if (player != null)
        {
            // 이번 프레임 동안 이동한 거리 계산
            Vector3 delta = transform.position - prevPos;
            // 플레이어를 그만큼 이동
            player.position += delta; 
        }
    }
}