using UnityEngine;

public class Flotation : MonoBehaviour
{
    [SerializeField] private float flotationForce = 5f;
    [SerializeField] private float maxUpVelocity = 2f;

    private bool action;

    private PlayerMovement _playerMovement;
    private Rigidbody2D _playerRb;

    private void FixedUpdate()
    {
        if (!action || _playerRb == null) return;

        // À§·Î ºÎÀ¯ Èû
        if (_playerRb.velocity.y < maxUpVelocity)
        {
            _playerRb.velocity = new Vector2(
                _playerRb.velocity.x,
                Mathf.Lerp(
                    _playerRb.velocity.y,
                    flotationForce,
                    Time.fixedDeltaTime * 3f
                )
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        action = true;
        _playerRb = collider.GetComponent<Rigidbody2D>();
        _playerMovement = collider.GetComponent<PlayerMovement>();
        _playerMovement.moveLimit = true;

        _playerRb.velocity = Vector2.zero;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        action = false;
        _playerMovement.moveLimit = false;
        _playerRb = null;
        _playerMovement = null;
    }
}
