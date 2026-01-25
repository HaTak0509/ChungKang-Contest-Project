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


    public static HumidityManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        _waterRiseController = GetComponent<WaterRiseController>();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
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
