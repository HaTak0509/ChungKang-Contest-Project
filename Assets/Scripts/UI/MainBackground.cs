using UnityEngine;
using UnityEngine.InputSystem;

public class MainBackground : MonoBehaviour
{
    [Header("설정")]
    private RectTransform targetUI;      // 움직일 배경 이미지의 RectTransform
    public float parallaxStrength = 20f; // 움직임 강도 (숫자가 커질수록 많이 움직임)
    public float smoothTime = 0.1f;     // 부드러운 이동 속도

    private Vector2 startAnchoredPos;   // UI의 초기 고정 위치
    private Vector2 currentVelocity;    // SmoothDamp용 속도 변수

    void Start()
    {
        targetUI = GetComponent<RectTransform>();


        if (targetUI == null)
        {
            targetUI = GetComponent<RectTransform>();
        }

        // UI의 초기 anchoredPosition을 저장합니다.
        startAnchoredPos = targetUI.anchoredPosition;
    }

    void Update()
    {
        // 화면 해상도 범위를 벗어났는지 체크
 

        if (targetUI == null) return;


        // 2. Input.mousePosition 대신 Mouse.current.position.ReadValue() 사용
        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (mousePos.x < 0 || mousePos.y < 0 ||
        mousePos.x > Screen.width || mousePos.y > Screen.height)
        {
           return;
        }

        // 나머지 로직은 동일
        float mouseOffsetX = (mousePos.x - Screen.width / 2f) / (Screen.width / 2f);
        float mouseOffsetY = (mousePos.y - Screen.height / 2f) / (Screen.height / 2f);

        Vector2 targetPos = new Vector2(
            startAnchoredPos.x + (mouseOffsetX * parallaxStrength),
            startAnchoredPos.y + (mouseOffsetY * parallaxStrength)
        );

        targetUI.anchoredPosition = Vector2.SmoothDamp(
            targetUI.anchoredPosition,
            targetPos,
            ref currentVelocity,
            smoothTime
        );
    }
}