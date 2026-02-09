using UnityEngine;

public class Decoration : MonoBehaviour
{
    public Sprite TwistSprite;
    private bool _enable = false;
    private SpriteRenderer _renderer;

    void Start()
    {
        if (!_enable)
        {
            GameObject clone = Instantiate(gameObject,transform);
            Destroy(clone.GetComponent<Decoration>());
            
            SpriteRenderer clone_renderer = clone.GetComponent<SpriteRenderer>();
            clone_renderer.sprite = TwistSprite;

            clone_renderer.flipX = _renderer.flipX;
            clone_renderer.flipY = _renderer.flipY;
            //잠깐 정지, 여기서는 특정 레이어에서 투과되게 할꺼임 ㅇㅇ


        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
