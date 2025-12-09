using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private bool hold;

    private Animator animator;
    private bool opened;

    void Awake()
    {
        animator = GetComponent<Animator>();
        opened = false;
    }

    public void OpenBridge()
    {
        if (hold)
        {
            // animator추가
            Debug.Log("OpenDoor");
        }
        else
        {
            // 일반 모드는 한 번 열리면 끝
            if (!opened)
            {
                Debug.Log("OpenDoor");
                opened = true;
                // animator추가
            }
        }
    }

    public void CloseBridge()
    {
        if (hold)
        {
            Debug.Log("CloseDoor");
            // animator추가
        }
    }
}
