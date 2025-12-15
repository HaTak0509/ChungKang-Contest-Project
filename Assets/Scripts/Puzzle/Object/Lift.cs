using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private bool active = false;
    private Rigidbody2D _rb2D;
    private BoxCollider2D _collider2D;

    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerController.intercation)f
            {
                StartCoroutine(Lifting());
            }
        }
    }
    private IEnumerator Lifting()
    {
        yield return null;
    }
}
