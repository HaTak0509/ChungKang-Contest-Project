using UnityEngine;

public class PlayerFacing : MonoBehaviour
{
    private Vector3 _baseScale;
    public bool IsFacingRight { get; private set; } = true;

    private void Awake()
    {
        _baseScale = transform.localScale;
    }

    public void FaceDirection(float inputX)
    {
        if (inputX == 0) return;

        bool faceRight = inputX > 0;
        if (faceRight != IsFacingRight)
        {
            IsFacingRight = faceRight;
            float x = faceRight ? _baseScale.x : -_baseScale.x;
            transform.localScale = new Vector3(x, _baseScale.y, _baseScale.z);
        }
    }
}