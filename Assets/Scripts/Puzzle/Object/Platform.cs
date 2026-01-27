using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Door targetDoor;
    [SerializeField] private Bridge targetBridge;

    [SerializeField] private float sinkAmount = 0.2f;
    [SerializeField] private float sinkSpeed = 2f;

    private Vector3 originalPosition;
    private Vector3 pressedPosition;

    private int pressCount = 0; // 몇 개가 밟고 있는지
    private Transform carriedObject; // 플레이어 or 박스

    void Start()
    {
        originalPosition = transform.position;
        pressedPosition = originalPosition + Vector3.down * sinkAmount;
    }

    void Update()
    {
        Vector3 prevPos = transform.position;

        Vector3 targetPos = pressCount > 0 ? pressedPosition : originalPosition;

        // 정확히 목표 위치까지 이동
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            sinkSpeed * Time.deltaTime
        );

        // 발판 위 오브젝트 같이 이동
        if (carriedObject != null)
        {
            Vector3 delta = transform.position - prevPos;
            carriedObject.position += delta;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsTopCollision(collision)) return;

        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("LightBox"))
        {
            pressCount++;

            carriedObject = collision.transform;

            if (pressCount == 1)
            {
                if (targetDoor != null) targetDoor.OpenDoor();
                if (targetBridge != null) targetBridge.OpenBridge();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("LightBox"))
        {
            pressCount = Mathf.Max(pressCount - 1, 0);

            if (pressCount == 0)
            {
                carriedObject = null;
                if (targetDoor != null) targetDoor.CloseDoor();
                if (targetBridge != null) targetBridge.CloseBridge();
            }
        }
    }

    bool IsTopCollision(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
                return true;
        }
        return false;
    }
}
