public interface TestRobotState // 로봇의 상태를 인퍼페이스로 나타내서 상태패턴에 이용.
{
    void Enter();
    void Update();
    void Exit();
}