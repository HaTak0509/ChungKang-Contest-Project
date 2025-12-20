using TMPro;
using UnityEngine;

public class SadState : IEmotionState
{
    //*************************************************************
    // [ 코드 설명 ] :
    // 감정에 따른 여러 행동방식을 유사한 구조로 작성해서, 유지보수와
    // 확장성을 챙기기 위해 인터페이스 형식을 사용함
    //*************************************************************

    public EmotionType Type => EmotionType.Sad;
    
    private GameObject Smoke;
    private ParticleSystem _particleSystem;

    public void OnEnter(Monster monster)
    {


        if (Smoke == null)
        {
            GameObject _smokePrefab = Resources.Load<GameObject>("Particle/SmokeParticle");
            Smoke = GameObject.Instantiate(_smokePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Smoke.transform.SetParent(monster.transform, false);
            _particleSystem = Smoke.GetComponent<ParticleSystem>();
        }

        _particleSystem.Play();
    }

    public void UpdateState(Monster monster)
    {


    }

    public void OnExit(Monster monster) 
    {
        _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}