using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class WaterRiseController : MonoBehaviour
{
    [Header("Tilemap")]
    [SerializeField] private Tilemap waterTilemap;
    [SerializeField] private TileBase waterTile;

    [Header("Water Settings")]
    [SerializeField] private float riseInterval = 1.0f; // 몇 초마다 상승
    [SerializeField] private int startY = -5;           // 시작 높이
    [SerializeField] private int maxY = 20;             // 최대 높이
    [SerializeField] private int minX = -20;
    [SerializeField] private int maxX = 20;

    private int currentY;

    void Start()
    {
        currentY = startY;
        StartCoroutine(WaterRiseRoutine());
    }

    private IEnumerator WaterRiseRoutine()
    {
        while (currentY <= maxY)
        {
            RaiseOneRow();
            currentY++;
            yield return new WaitForSeconds(riseInterval);
        }
    }

    private void RaiseOneRow()
    {
        for (int x = minX; x <= maxX; x++)
        {
            Vector3Int cellPos = new Vector3Int(x, currentY, 0);
            waterTilemap.SetTile(cellPos, waterTile);
        }
    }
}
