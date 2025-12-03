using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private float pressDistance = 0.1f;
    [SerializeField] private float pressSpeed= 0.75f;

    private Vector3 _originalPos;
    private bool _isPressed = false;

    void Start()
    {
        _originalPos = transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPressed = false;
        }
    }

    private void Update()
    {
        if (_isPressed)
        {
            Vector3 target = _originalPos + Vector3.down * pressDistance;
            transform.localPosition = Vector3.Lerp(transform.position, target, pressSpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.position, _originalPos, pressSpeed * Time.deltaTime);
        }
    }
}
