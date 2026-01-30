using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierDevice : MonoBehaviour, IInteractable
{


    [Header("기본 설정")]
    public float baseDetectionRange = 3f;

    public float InteractRange { get; private set; }
    private string hexColor = "#00B2FF";
    private DrawSensingRange _lineRenderer;
    private bool _enable = false;


    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<DrawSensingRange>();
        InteractRange = baseDetectionRange;

        _lineRenderer.OnLine();

        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            Debug.Log(newColor.ToString());
            _lineRenderer.Draw(InteractRange, newColor);
        }
    }

    public void Interact()
    {
        _enable = !_enable;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        // 에디터에서 범위를 보기 쉽게 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y), baseDetectionRange);
    }
}
