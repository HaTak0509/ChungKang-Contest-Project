using System.Collections;
using UnityEngine;

public class LiftController : MonoBehaviour
{
    [Header("움직임 설정")]
    [SerializeField] private float speed = 3f;  // 이동 속도
    [SerializeField] private float downY = -5f; // Inspector에서 땅에 닿을 Y 위치 설정 (예: -5)

    private Vector2 originalPos;  // 원래 위 위치
    private bool atBottom = false;  // 아래에 있는지 여부
    private bool playerOnLift = false;  // Player가 타고 있는지
    private bool isMoving = false;  // 이동 중인지
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPos = transform.position;  // 시작 위치를 원래 위치로 저장
    }

    void Update()
    {
        // F키 누르고, Player가 타고 있고, 이동 중이 아닐 때만 동작
        if (Input.GetKeyDown(KeyCode.F) && playerOnLift && !isMoving)
        {
            Vector2 target;
            if (atBottom)
            {
                // 아래에서 위로 올라감
                target = originalPos;
                atBottom = false;
            }
            else
            {
                // 위에서 아래로 내려감
                target = new Vector2(originalPos.x, downY);
                atBottom = true;
            }
            StartCoroutine(MoveTo(target));
        }
    }

    // 부드럽게 목표 위치로 이동 (jitter 방지 위해 MovePosition 사용)
    IEnumerator MoveTo(Vector2 target)
    {
        isMoving = true;

        while (Vector2.Distance(rb.position, target) > 0.01f)
        {
            rb.MovePosition(
                Vector2.MoveTowards(
                    rb.position,
                    target,
                    speed * Time.fixedDeltaTime
                )
            );
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(target);
        isMoving = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        playerOnLift = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        playerOnLift = false;
    }
}