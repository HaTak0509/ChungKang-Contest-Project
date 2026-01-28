using UnityEngine;

public class Pushing : MonoBehaviour
{
    public PushingObject pushingOb;
    public float pushSpeed = 4f;

    private float inputX;
    private bool isPushing;

    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.F))
        {
            isPushing = !isPushing;
        }
    }

    private void FixedUpdate()
    {
        if (pushingOb == null)
            return;

        // 밀기 상태가 아니면 정지
        if (!isPushing || Mathf.Abs(inputX) < 0.01f)
        {
            pushingOb.Stop();
            return;
        }

        pushingOb.Push(new Vector2(inputX * pushSpeed, 0f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LightBox"))
        {
            pushingOb = collision.GetComponent<PushingObject>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("LightBox"))
        {
            pushingOb = null;
        }
    }
}
