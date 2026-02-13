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
        
    }

    private void Update()
    {
        float target = (_Humidity != 0 && _waterRiseController.CurrentStep >= 1) ? _maxTargetValue : 0f;

        m_AudioSource.volume = Mathf.MoveTowards(m_AudioSource.volume, target, 1 * Time.deltaTime);
    }




    public void UpHumidity()
    {
        _Humidity += 1;
        CheckHumidity();
    }

    public void DownHumidity()
    {
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
        else
        {
//            _waterRiseController.StopMovement();
        }

    }
}
