using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRobot : MonoBehaviour
{
    [SerializeField] private string robotType;
    [Space]
    [SerializeField] private float Speed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float dashSpeed;
    private TestRobotManager testRobotManager;

    void Start()
    {
        testRobotManager = GetComponentInParent<TestRobotManager>();

        if (robotType == "Empty")
        {

        }
        else if (robotType == "Happy") // 타입을 영어로 적고 그 타입에 맞춰서 로봇의 상태 설정
        {
            testRobotManager.SetState(new TestHappy());
        }
        else if (robotType == "Sad")
        {

        }
        else if (robotType == "Angry")
        {

        }
        else if (robotType == "Fear")
        {

        }
        else if (robotType == "Fond")
        {

        }
        else if (robotType == "Jealousy")
        {

        }
        else if (robotType == "Thrill")
        {

        }
        else if (robotType == "Resentment")
        {

        }
        else if (robotType == "Thrill")
        {

        }
        else if (robotType == "Anxiety")
        {

        }
        else if (robotType == "Tension")
        {

        }
        else if (robotType == "Madness")
        {

        }
        else if (robotType == "Screams")
        {

        }
        else if (robotType == "Frenzy")
        {

        }
    }

    void Update()
    {
        
    }
}
