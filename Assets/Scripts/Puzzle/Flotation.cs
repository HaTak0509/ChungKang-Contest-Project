using UnityEngine;
using UnityEngine.UIElements;

public class Flotation : MonoBehaviour
{
    [SerializeField] private float flotationForce = 5f;
    [SerializeField] private float maxPosition = 2f;

    private bool action;

    private PlayerMovement _playerMovement;
    private Rigidbody2D _playerRb;
    private Transform _playerPosition;

    private void FixedUpdate()
    {
        if (!action || _playerRb == null) return;

        // À§·Î ºÎÀ¯ Èû
        if (_playerPosition.position.y < maxPosition)
        _playerRb.velocity = new Vector2(
            _playerRb.velocity.x,
            Mathf.Lerp(_playerRb.velocity.y, flotationForce, Time.fixedDeltaTime * 3f));
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        action = true;
        _playerPosition = collider.gameObject.transform;
        _playerRb = collider.GetComponent<Rigidbody2D>();
        _playerMovement = collider.GetComponent<PlayerMovement>();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        action = false;
        _playerPosition = null;
        _playerRb = null;
        _playerMovement = null;
    }
}
