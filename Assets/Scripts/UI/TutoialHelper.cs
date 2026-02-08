using System.Collections.Generic;
using UnityEngine;

public class TutorailHelper : MonoBehaviour
{
    [TextArea(3, 10)] // 최소 3줄, 최대 10줄 크기
    public string _Description;
    public GameObject _gameObject;

    private GameObject _container;

    private static readonly int _OutlineWidthID = Shader.PropertyToID("_OutlineWidth");
    [SerializeField] private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>(); // 검사할 영역의 위치
    [SerializeField] private Vector2 checkPos = new Vector2(2.7f, 1.5f); // 검사할 영역의 위치
    [SerializeField] private Vector2 checkSize = new Vector2(2.7f, 2.7f); // 검사할 영역의 크기


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _container = Instantiate(_gameObject);
            _container.transform.position = (Vector2)transform.position + checkPos;
            _container.GetComponent<Lore>().SetText(_Description);
        }

        foreach (SpriteRenderer sprite in _spriteRenderers)
        {
            MaterialPropertyBlock _mpb = new MaterialPropertyBlock();
            sprite.GetPropertyBlock(_mpb);

            _mpb.SetFloat(_OutlineWidthID, 1);

            sprite.SetPropertyBlock(_mpb);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (SpriteRenderer sprite in _spriteRenderers)
            {
                MaterialPropertyBlock _mpb = new MaterialPropertyBlock();
                sprite.GetPropertyBlock(_mpb);

                _mpb.SetFloat(_OutlineWidthID, 0);

                sprite.SetPropertyBlock(_mpb);
            }

            Destroy(_container);
        }
    }


    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.magenta;

        Gizmos.DrawWireCube((Vector2)transform.position + checkPos, checkSize);
    }

}
