using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool hold;

    private Animator animator;
    private Collider2D collider2D;
    private bool opened;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
        collider2D.isTrigger = false;
        opened = false;
    }

    public void OpenDoor()
    {
        if (hold)
        {
            // hold 모드에서는 단순히 트리거 ON
            collider2D.isTrigger = true;
        }
        else
        {
            // 일반 모드는 한 번 열리면 끝
            if (!opened)
            {
                opened = true;
                collider2D.isTrigger = true;
            }
        }
    }

    public void CloseDoor()
    {
        // hold 모드일 때만 닫힘
        if (hold)
        {
            collider2D.isTrigger = false;
        }
    }
}