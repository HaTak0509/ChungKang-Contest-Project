using UnityEngine;

public class PlayerFacing : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Pushing _pushing;

    public bool IsFacingRight { get; private set; } = true;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _pushing = GetComponent<Pushing>();
    }

    public void FaceDirection(float inputX)
    {
        if (inputX == 0 || _pushing.isPushing) return;

        bool faceRight = inputX > 0;
        _spriteRenderer.flipX = !faceRight;
        IsFacingRight = faceRight;
    }
}