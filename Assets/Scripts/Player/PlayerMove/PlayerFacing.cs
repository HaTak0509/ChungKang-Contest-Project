using UnityEngine;

public class PlayerFacing : MonoBehaviour
{
    private Vector3 baseScale;
    public bool IsFacingRight { get; private set; } = true;

    private void Awake() => baseScale = transform.localScale;

    public void FaceDirection(float inputX)
    {
        if (inputX == 0) return;

        bool faceRight = inputX > 0;
        if (faceRight != IsFacingRight)
        {
            IsFacingRight = faceRight;
            float x = faceRight ? baseScale.x : -baseScale.x;
            transform.localScale = new Vector3(x, baseScale.y, baseScale.z);
        }
    }
}