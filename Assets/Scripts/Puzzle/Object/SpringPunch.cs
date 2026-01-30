using UnityEngine;

public class SpringPunch : MonoBehaviour
{
    public Animator _animator;
    public float Force;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("플레이어와 충돌");
            collision.transform.GetComponent<Damageable>().TakePushFromPosition(transform.position,Force);
            _animator.SetTrigger("Action");
        }
    }


}
