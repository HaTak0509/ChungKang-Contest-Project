using Unity.VisualScripting;
using UnityEngine;

public class PlayerScale : MonoBehaviour
{
    Vector3 _OriginScale;
    [SerializeField] float _Scale = 100;
    float _MaxScale = 100;
    float timer = 0f;
    

    public static PlayerScale Instance;
    public bool _isDown = false;

    // Start is called before the first frame update
    void Start()
    {
        _OriginScale = transform.localScale;
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = _OriginScale * _Scale / _MaxScale;
        transform.localScale = scale;

        if (!_isDown && _Scale < 100)
        {
            // 1. 타이머가 3초보다 작을 때만 시간을 더함
            if (timer < 1f)
            {
                timer += Time.deltaTime; // 프레임 사이의 시간을 누적

                AddScale(2f);
            }
            else
            {
                timer = 0f;
            }
        }
        else if (_isDown && _Scale > 20)
        {  
            // 1. 타이머가 3초보다 작을 때만 시간을 더함
            if (timer < 1f)
            {
                timer += Time.deltaTime; // 프레임 사이의 시간을 누적

                AddScale(-4f);
            }
            else
            {
                timer = 0f;
            }
        }


    }

    public void AddScale(float AddScale)
    {
        _Scale += AddScale;
    }
}