using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRobotManager : MonoBehaviour
{
    private TestRobotState _currentState;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetState(TestRobotState newState) // 로봇의 현재 상태 바꾸기
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
