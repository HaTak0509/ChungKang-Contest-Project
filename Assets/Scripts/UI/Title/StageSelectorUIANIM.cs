using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectorUIANIM : MonoBehaviour
{
    private Animator _SceneAnimator;

    private void Awake()
    {
        _SceneAnimator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        _SceneAnimator.Play("StartChapter");
    }
}
