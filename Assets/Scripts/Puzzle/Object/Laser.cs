using UnityEngine;

public class Laser : MonoBehaviour, WarpingInterface
{
    [SerializeField] private PlayerDash _playerDash;
    [SerializeField] private GameObject _warpingLaser;

    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (_playerDash.dashing)
        {
            _boxCollider.isTrigger = true;
        }
        else
        {
            _boxCollider.isTrigger = false;
        }
    }

    public void Warping()
    {
        if (_warpingLaser != null)
        {
            _warpingLaser.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
