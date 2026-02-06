using UnityEngine;

public class Fire : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //삭제
            Debug.Log("어!? 불에 닿았네!??!? 죽어 그럼");

            collision.transform.GetComponent<Damageable>().GameOver();
        }
    }
}
