using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private Vector3 _originalposition;
    private float _liftDuration;
    private float _liftSpeed;
    private bool active = false;
    
    void Start()
    {
        _originalposition = transform.position;

    }

    void Update()
    {
        if (playerController.intercation && active)
        {

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            active = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            active = false;
        }
    }

    private IEnumerator Lifting()
    {
        yield return null;
    }
}
