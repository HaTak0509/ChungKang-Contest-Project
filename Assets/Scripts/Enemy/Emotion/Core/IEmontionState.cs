public interface IEmotionState//인터페이스 정의
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 몬스터의 감정에 따른 여러 행동방식이 존재해야하므로
    // 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************


    EmotionType Type { get; }
    void OnEnter(Monster monster);
    void OnExit(Monster monster);
    void UpdateState(Monster monster);
}
