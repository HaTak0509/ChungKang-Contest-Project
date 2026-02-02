using UnityEngine;


//*************************************************************
// [ 코드 설명 ] :
// 습도 시스템 관리자
// 
//*************************************************************

public class HumidityManager : MonoBehaviour
{
    private WaterRiseController _waterRiseController;
    [SerializeField] private int _Humidity = 0;
    public int Humidity => _Humidity;

    public static HumidityManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        _waterRiseController = GetComponent<WaterRiseController>();
        
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
            _waterRiseController.StopMovement();
        }

    }
}
