using UnityEngine;

public class GetItem : MonoBehaviour
{
    [Header("아이템 데이터")]
    [SerializeField] private ItemData itemData;

    private bool _active;

    private void Start()
    {
        // 이미 먹은 아이템이면 제거
        if (PlayerPrefs.GetInt(itemData.itemID, 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_active && Input.GetKeyDown(KeyCode.F))
        {
            ItemBase.Instance.AddItem(itemData);

            PlayerPrefs.SetInt(itemData.itemID, 1);
            PlayerPrefs.Save();

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        _active = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        _active = false;
    }
}
