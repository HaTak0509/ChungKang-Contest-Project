using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;


//*************************************************************
// [ 코드 설명 ] :
// 습도 시스템 관리자
// 
//*************************************************************

public class HumidityManager : MonoBehaviour
{
    [SerializeField] AudioSource m_AudioSource;
    private WaterRiseController _waterRiseController;
    [SerializeField] private int _Humidity = 0;
    [SerializeField] private float _maxTargetValue = 0.1f;
    public int Humidity => _Humidity;

    public static HumidityManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        _waterRiseController = GetComponent<WaterRiseController>();
        StageClear().Forget();
    }

    private void Update()
    {
        float target = (_Humidity != 0 && _waterRiseController.CurrentStep >= 1) ? _maxTargetValue : 0f;

        m_AudioSource.volume = Mathf.MoveTowards(m_AudioSource.volume, target, 1 * Time.deltaTime);
    }
    public async UniTask StageClear()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        FindEnemiesInArea();
    }


    public void FindEnemiesInArea()
    {
        _Humidity = 0;
        int layerMask = LayerMask.GetMask("Enemy");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(Vector2.zero, 100f, layerMask);

        foreach (var enemyCollider in hitEnemies)
        {
            if(enemyCollider.GetComponent<ScreamiingState>() != null)
            {
                _Humidity += 1;
                Debug.Log(enemyCollider.name + " UP");
            }

            if (enemyCollider.GetComponent<OutrageState>() != null)
            {
                _Humidity -= 1;
                Debug.Log(enemyCollider.name + " DOWN");
            }
        }
        Debug.Log("---");
        CheckHumidity();
    }

    public void UpHumidity()
    {
        Debug.Log("증가");
        _Humidity += 1;
        CheckHumidity();
    }

    public void DownHumidity()
    {
        Debug.Log("감소");
        _Humidity -= 1;
        CheckHumidity();
    }

    private void CheckHumidity()
    {
        if (_Humidity > 0)
        {
            _waterRiseController.StartRising();
        }
        else if (_Humidity < 0)
        {
            _waterRiseController.StartFalling();
        }

    }
}
