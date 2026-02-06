using System.Collections.Generic;
using UnityEngine;



public class HelperManager : MonoBehaviour
{
    [SerializeField] private bool _enable = false;
    [SerializeField] private Transform _Dark;
    private Camera _camera;

    [SerializeField] private List<SpriteRenderer> _HelpList;
    private List<Material> _HelpList2;

    private HashSet<Helper> _helpers = new HashSet<Helper>();
    private PlayerController _player;
    private static readonly int _OutlineWidthID = Shader.PropertyToID("_OutlineWidth");

    void Start()
    {
        _camera = GetComponent<Camera>();
        if (_player == null)
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _enable = !_enable;

            Time.timeScale = _enable ? 0.0f : 1.0f;
            _Dark.gameObject.SetActive(_enable);
            _player.allLimit = _enable;

            foreach (SpriteRenderer sprite in _HelpList)
            {
                sprite.sortingOrder = _enable ? 17 : 0;


                if (sprite.CompareTag("Player")) continue;

                MaterialPropertyBlock _mpb = new MaterialPropertyBlock();
                sprite.GetPropertyBlock(_mpb);

                _mpb.SetFloat(_OutlineWidthID, _enable ? 1 : 0);
                Debug.Log(123);
                
                sprite.SetPropertyBlock(_mpb);

            }


            if (!_enable)
            {
                foreach (Helper helper in _helpers)
                {
                    helper.Remove();
                    helper._isEnable = false;
                }
                _helpers.Clear();
                return;
            }

            FullScreen();


        }

        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            if (!_enable) return;

            Debug.Log("누름");
            // 1. 마우스 위치를 월드 좌표로 변환
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 2. 해당 위치에 있는 콜라이더 감지
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log($"감지된 오브젝트: {hit.collider.name}");


                // 예: 몬스터인지 확인하고 기능 실행
                if (hit.collider.GetComponent<Helper>() != null)
                {
                    _helpers.Add(hit.collider.GetComponent<Helper>());
                    hit.collider.GetComponent<Helper>().ToggleLore();
                }
            }
        }


    }
    void FullScreen()
    {
        SpriteRenderer sr = _Dark.GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // 1. 초기 초기화
        _Dark.localScale = Vector3.one;

        // 2. 스프라이트의 원래 크기(유닛 단위) 가져오기
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        // 3. 카메라의 높이와 너비 계산 (World Unit 기준)
        float worldScreenHeight = _camera.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // 4. 새로운 스케일 적용
        Vector3 newScale = transform.localScale;
        newScale.x = worldScreenWidth / width;
        newScale.y = worldScreenHeight / height;

        _Dark.localScale = newScale;

        // 카메라 정중앙으로 배치
        _Dark.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}