using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Door targetDoor;

    private int _pressCount = 0; // 몇 개가 밟고 있는지
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsTopCollision(collision)) return;

        if (collision.gameObject.CompareTag("LightBox"))
        {
            _pressCount++;

            _animator.SetBool(AnimationStrings.OnPlatform, true);

            if (_pressCount == 1)
            {
                if (targetDoor != null) targetDoor.OpenDoor();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LightBox"))
        {
            _pressCount = Mathf.Max(_pressCount - 1, 0);

            _animator.SetBool(AnimationStrings.OnPlatform, false);

            if (_pressCount == 0)
            {
                if (targetDoor != null) targetDoor.CloseDoor();
            }
        }
    }

    bool IsTopCollision(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
                return true;
        }
        return false;
    }
}
