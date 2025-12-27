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
        for (int i = 0; i < segments; i++)
        {
            float angle = i * 2 * Mathf.PI / segments;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    }
}
