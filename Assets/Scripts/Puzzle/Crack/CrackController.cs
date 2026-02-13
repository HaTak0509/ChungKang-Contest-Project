
using System.Collections.Generic;
using UnityEngine;

public class CrackController : MonoBehaviour, IInteractable
{

    [Header("기본 설정")]
    public float baseDetectionRange = 3f;
    public bool _IsEnable = false;

    public float InteractRange { get; private set; }
    private string hexColor = "#D400FF";
    private DrawSensingRange _lineRenderer;
    private Animator _animator;

    private bool _enabled = false;

    private List<Crack> cracks = new List<Crack>();

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _lineRenderer = GetComponent<DrawSensingRange>();
        InteractRange = baseDetectionRange;

        _lineRenderer.OnLine();

        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            Debug.Log(newColor.ToString());
            _lineRenderer.Draw(InteractRange, newColor);
        }

        SetCrack();


        if (_IsEnable) Interact();
    }

    public void Interact()
    {
        CrackActive();
    }

    void SetCrack()
    {

        Collider2D[] hitcrack = Physics2D.OverlapCircleAll(transform.position, InteractRange, LayerMask.GetMask("Crack"));

        if (hitcrack.Length == 0) return; // 3블럭 이내 AI가 없으면 종료

        foreach (var crack in hitcrack)
        {
            cracks.Add(crack.GetComponent<Crack>());
        }
    }

    void ToggleCrack()
    {
        foreach (var crack in cracks)
        {
            crack.SetCrack(!crack.isActivated);
        }
    }

    public void CrackActive()
    {
        _enabled = !_enabled;
        if (_enabled)
            _animator.SetTrigger("ON");
        else
            _animator.SetTrigger("OFF");

        ToggleCrack();
        Debug.Log(123);
    }


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
