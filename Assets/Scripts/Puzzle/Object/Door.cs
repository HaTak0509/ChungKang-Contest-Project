using UnityEngine;

public class Door : MonoBehaviour
{
    public bool currentState;

    [Header("문 애니메이션 설정")]
    public GameObject OpenOBJ;
    public Sprite _Open;
    public Sprite _Close;

    private SpriteRenderer _spriteRenderer;
    private new Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.isTrigger = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OpenDoor()
    {
        currentState = true;
        collider.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Default");

        
        _spriteRenderer.sprite = _Open;
        OpenOBJ.SetActive(true);
    }

    public void CloseDoor()
    {
        currentState = false;
        collider.isTrigger = false;
        gameObject.layer = LayerMask.NameToLayer("Ground");

        _spriteRenderer.sprite = _Close;
        OpenOBJ.SetActive(false);
    }
}