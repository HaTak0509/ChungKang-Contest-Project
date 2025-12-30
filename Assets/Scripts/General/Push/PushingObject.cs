using UnityEngine;
public class PushingObject : MonoBehaviour 
{
    private Rigidbody2D rb; 

    private void Awake() 
    { 
        rb = GetComponent<Rigidbody2D>();
    } 

    public void Push(Vector2 direction)
    {
        rb.velocity = new Vector2(direction.x, rb.velocity.y).normalized;
    }
    
    public void Stop()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    } 
}