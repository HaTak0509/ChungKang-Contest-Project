using UnityEngine;

public class Pushing : MonoBehaviour
{
    public bool isPushing;

    private PushingObject pushingObj;
    private Rigidbody2D playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (pushingObj == null || !pushingObj.isActive) return;

        float playerVelX = playerRb.velocity.x;

        // 플레이어가 멈추면 상자도 멈춤
        if (Mathf.Abs(playerVelX) < 0.01f)
        {
            isPushing = false;
            pushingObj.Stop();
            return;
        }

        // 플레이어 → 상자 방향 확인
        float toBox = pushingObj.transform.position.x - transform.position.x;

        // 상자 쪽으로 움직일 때만 복사
        if (Mathf.Sign(playerVelX) == Mathf.Sign(toBox))
        {
            isPushing = true;
            pushingObj.CopyVelocity(playerVelX);
        }
        else
        {
            // 반대 방향이면 즉시 분리
            isPushing = false;
            pushingObj.Stop();
            pushingObj = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PushingObject obj))
        {
            pushingObj = obj;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PushingObject obj))
        {
            if (pushingObj == obj)
            {
                pushingObj.Stop();
                pushingObj = null;
            }
        }
    }
}
    
