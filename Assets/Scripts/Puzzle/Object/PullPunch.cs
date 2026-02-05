using UnityEngine;
using Cysharp.Threading.Tasks;

public class PullPunch : MonoBehaviour, WarpingInterface
{
    [Header("감지 설정")]
    [SerializeField] private Vector3 detectionPos = new Vector3(); // 감지할 사각형 위치
    [SerializeField] private Vector2 detectionSize = new Vector2(5f, 2f); // 감지할 사각형 크기

    [SerializeField] private Vector3 hitPos = new Vector3(); // 감지할 사각형 위치
    [SerializeField] private Vector2 hitSize = new Vector2(5f, 2f); // 감지할 사각형 크기

    [SerializeField] private LayerMask targetLayer;                     // 감지할 레이어 (Player 등)

    [Header("설정")]
    [SerializeField] private int maxResistance = 3;      // 탈출에 필요한 저항 횟수
    [SerializeField] private float resistCooldown = 0.2f; // 저항 입력 쿨타임
    [SerializeField] private float pullSpeed = 5.0f;      // 플레이어를 당기는 속도
    [SerializeField] private float thornSpeed = 15.0f;    // 가시 발사 속도
    [SerializeField] private float ReadyTime = 0.5f; //대기 시간
    [SerializeField] private float CoolTime = 1.5f; //쿨타임


    [Header("필수 연결")]
    [SerializeField] private Transform _Niddle;      // 발사할 가시 프리팹
    [SerializeField] private Transform firePoint;         // 가시 발사구
    [SerializeField] private SpriteRenderer Hand;         // 손
    [SerializeField] private Sprite GrapHand;
    [SerializeField] private Sprite OpenHand;
    public GameObject TwistObject;

    [Header("회전")]
    public bool _IsRight = true;


    [HideInInspector] public bool _isFirst = false;
    private float _PlayerGravity = 0f;
    private Vector3 OriginHand;
    private LineRenderer chainLine;
    private bool _isPulling = false;
    private bool _isReturn = false;
    private int _currentResistCount = 0;
    private float _lastResistTime = 0f;
    private Transform _caughtPlayer;
    private float direction;
        
    private void Start()
    {
        chainLine = GetComponent<LineRenderer>();

        direction = _IsRight == true ? 1f : -1f;
        transform.localScale = new Vector3(transform.localScale.x * direction, transform.localScale.y, transform.localScale.z);

        OriginHand = _Niddle.position;

        if (TwistObject != null && !_isFirst)
        {
            TwistObject = Instantiate(TwistObject);
            TwistObject.transform.SetParent(transform.parent);
            TwistObject.SetActive(false);
            _isFirst = true;
            TwistObject.GetComponent<SpringPunch>()._isFirst = true;
            TwistObject.GetComponent<SpringPunch>()._IsRight = _IsRight;
            TwistObject.GetComponent<SpringPunch>().TwistObject = gameObject;
        }
    }

    public void Warping()
    {
        if (TwistObject == null) return;


        TwistObject.SetActive(true);

        TwistObject.transform.position = transform.position;

        gameObject.SetActive(false);

    }


    void Update()
    {
        if (!_isPulling)
            CheckForTargets();
        else
            DrawChain();

        chainLine.enabled = _isPulling;

        if (_isPulling && _caughtPlayer != null)
        {
            HandleResistance();
        }

        if(_isReturn) PullPlayer();
    }

    // 내가 원할 때 호출하는 함수
    private async void CheckForTargets()
    {
        // 현재 위치에서 detectionSize만큼의 사각형 안에 있는 모든 콜라이더를 가져옴
        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(transform.position + new Vector3(detectionPos.x * direction, detectionPos.y, 0), detectionSize, 0f, targetLayer);

        if (hitTargets.Length > 0)
        {
            foreach (var target in hitTargets)
            {
                Debug.Log($"{target.name} 감지 성공!");

                if (!_isPulling)
                {
                    _isPulling = true;
                    await WaitPunch();
                }
            }
        }
        else
        {
            Debug.Log("범위 내에 아무도 없습니다.");
        }
    }

    private async UniTask WaitPunch()
    {
        Debug.Log("ㄱㄷ");
        await UniTask.WaitForSeconds(ReadyTime);
        
        FireThorn();
    }

    private async UniTask ReturnThorn()
    {
        Debug.Log("아잇 못 맞췄네, 시마이치자");
        Hand.sprite = GrapHand;
        await UniTask.WaitForSeconds(CoolTime);

        Debug.Log("집에 가자");


        while (Vector3.Distance(_Niddle.position, OriginHand) > 0.1f)
        {
            // 가시를 본체(발사구) 방향으로 이동
            _Niddle.position = Vector3.MoveTowards(
                _Niddle.position,
                OriginHand,
                thornSpeed * Time.deltaTime // 돌아올 때는 발사 속도와 맞추는 게 자연스럽습니다.
            );

            await UniTask.Yield(); // 다음 프레임까지 대기
        }


        _isPulling = false;

        Debug.Log("가시 회수 완료");
    }


    public async void FireThorn()
    {
        // 가시 생성
        Hand.sprite = OpenHand;
        _Niddle.position = OriginHand;

        // 가시가 날아갈 목표 지점 (현재 감지된 플레이어의 위치)
        // CheckForTargets에서 찾은 대상을 타겟으로 삼습니다.
        Vector3 targetPos = _Niddle.position + new Vector3(detectionPos.x * direction, detectionPos.y, 0);
        targetPos.x += 2.5f * direction;

        // [아이디어] 가시가 목표까지 날아가는 과정
        bool hitPlayer = await MoveThornToTarget(_Niddle, targetPos);

        if (hitPlayer)
        {
            Debug.Log("가시가 타겟에 명중함!");
        }
        else
        {//다시 못 맞춤
            await ReturnThorn();
        }
    }
        
    private async UniTask<bool> MoveThornToTarget(Transform thorn, Vector3 target)
    {
        while (Vector3.Distance(thorn.position, target) > 0.1f)
        {
            // 가시 이동
            thorn.position = Vector3.MoveTowards(thorn.position, target, thornSpeed * Time.deltaTime);

            // [중요] 이동 중에 플레이어와 충돌했는지 실시간 체크
            Collider2D hit = Physics2D.OverlapCircle(thorn.position, 0.5f, targetLayer);
            if (hit != null)
            {
                OnPlayerCaught(hit.transform);
                return true; // 잡기 성공
            }

            await UniTask.Yield(); // 다음 프레임까지 대기
            if (thorn == null) return false;
        }
        return false; // 끝까지 갔는데 못 잡음
    }


    // 2. 가시가 플레이어와 충돌했을 때 호출되는 함수
    public void OnPlayerCaught(Transform player)
    {
        _caughtPlayer = player;

        _PlayerGravity = _caughtPlayer.GetComponent<Rigidbody2D>().gravityScale;
        _caughtPlayer.GetComponent<Rigidbody2D>().gravityScale = 0;
        PlayerController.Instance.allLimit = true;


        _isReturn =  true;
        _isPulling = true;
        _currentResistCount = 0;
        
        Hand.sprite = GrapHand;
    }

    // 3. 저항 로직 (Space bar)
    private void HandleResistance()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= _lastResistTime + resistCooldown)
        {
            _currentResistCount++;
            _lastResistTime = Time.time;

            // 시각적/청각적 피드백 (화면 흔들림 등)
            Debug.Log($"저항 중! ({_currentResistCount}/{maxResistance})");

            if (_currentResistCount >= maxResistance)
            {
                ReleasePlayer();
            }
        }
    }

    // 4. 플레이어 끌어당기기
    private void PullPlayer()
    {
        _Niddle.position = Vector3.MoveTowards(_Niddle.position, OriginHand, pullSpeed * Time.deltaTime);

        if (_caughtPlayer != null) _caughtPlayer.position = _Niddle.position;
          

        if (Vector3.Distance(_Niddle.position, OriginHand) < 0.01f)
        {
            StopPull();
        }
    }

    // 5. 이건 풀려남
    private void ReleasePlayer()
    { 
        
        PlayerController.Instance.allLimit = false;
        _caughtPlayer.GetComponent<Rigidbody2D>().gravityScale = _PlayerGravity;

        _caughtPlayer = null;

        Debug.Log("플레이어를 놓아주었습니다.");
    }

    private void StopPull()
    {
        _isPulling = false;
        _isReturn = false;

        if(_caughtPlayer != null)
        {
            Debug.Log("넌 이미 죽어있다!");
        }
    }

    private void DrawChain()
    {
        chainLine.positionCount = 2; // 점 2개 (시작과 끝)

        // A지점: 몬스터 발사구
        chainLine.SetPosition(0, firePoint.position);

        // B지점: 현재 날아가고 있는 가시의 위치
        chainLine.SetPosition(1, _Niddle.position);
    }


    // 에디터에서 범위를 보기 위한 시각화
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        float direction = _IsRight == true ? 1f : -1f;
        Gizmos.DrawWireCube(transform.position + new Vector3(detectionPos.x * direction, detectionPos.y, 0), detectionSize);


        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(hitPos.x * direction, hitPos.y, 0), hitSize);
    }

}