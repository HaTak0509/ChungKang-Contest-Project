using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private float pressDistance;
    [SerializeField] private float pressSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            
        }
    }

    private IEnumerator Pressed ()
    {
        
        yield return null;
    }
}