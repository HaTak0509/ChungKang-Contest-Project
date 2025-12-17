using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDetection : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDistance = 0.05f;
    [SerializeField] private float wallDistance = 0.2f;

    private Animator _animator;
    private CapsuleCollider2D _capsuleCollider;
    private RaycastHit _raycastHit;

    private bool _isGround = false;

    public bool IsGround
    {
        get { return _isGround; }
        set 
        {
            _isGround = value; 
        }
    }

    private bool _isOnWall = false;

    public bool IsOnWall
    {
        get { return _isOnWall; }
        set 
        {
            _isOnWall = value; 
        }
    }

    void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 center = _capsuleCollider.bounds.center;
        float yDistance = _capsuleCollider.bounds.extents.y + groundDistance;
        float xDistance = _capsuleCollider.bounds.extents.x + wallDistance;

        IsOnWall = Physics2D.Raycast(center, Vector2.right * transform.localScale.x, xDistance, groundLayer);
        IsGround = Physics2D.Raycast(center, Vector2.down, yDistance, groundLayer);
    }
}
