using UnityEngine;

public class PlayerFacing : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public bool IsFacingRight { get; private set; } = true;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FaceDirection(float inputX)
    {
        if (inputX == 0) return;

        bool faceRight = inputX > 0;
        _spriteRenderer.flipX = !faceRight;
        IsFacingRight = faceRight;
    }
}