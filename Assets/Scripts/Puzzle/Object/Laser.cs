using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, WarpingInterface
{
    [SerializeField] private PlayerDash _playerDash;
    [SerializeField] private GameObject _warpingLaser;

    private void Update()
    {
        if (_playerDash.dashing)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Ground");
        }
    }

    public void Warping()
    {
        if (!_warpingLaser)
        {
            _warpingLaser.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
