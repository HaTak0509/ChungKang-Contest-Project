using UnityEngine;

public class DrawSensingRange : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public int segments = 50; // 원을 구성하는 선의 개수


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments;
        OffLine();
    }

    public void OnLine()
    {
        lineRenderer.enabled = true;
    }
    public void OffLine()
    {
        lineRenderer.enabled = false;
    }

    public void Draw(float radius, Color color)
    {
        // 부모의 스케일 값을 가져옴 (0 방지)
        float lossyX = transform.lossyScale.x == 0 ? 1 : transform.lossyScale.x;
        float lossyY = transform.lossyScale.y == 0 ? 1 : transform.lossyScale.y;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * 2 * Mathf.PI / segments;
            // 부모 스케일만큼 나눠서 크기를 상쇄시킴
            float x = (Mathf.Cos(angle) * radius) / lossyX;
            float y = (Mathf.Sin(angle) * radius) / lossyY;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
