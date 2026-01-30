using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;

public class TransparencyUI : MonoBehaviour
{
    public static TransparencyUI Instance { get; private set;}

    [Header("Scale_ UI")]
    public RectTransform trBar;
    public GameObject trUI;

    private float _MaxTrHeight;
    private PlayerColor _playerColor;


    private void Awake()
    {
        Instance = this;

        _playerColor = GetComponent<PlayerColor>();

        _MaxTrHeight = trBar.sizeDelta.y;
        trBar.sizeDelta = new Vector2(trBar.sizeDelta.x, _MaxTrHeight);
    }

    public void CheckTransparencyBar()
    {
        float currentTransparency = _playerColor.color.a;
    }
}
