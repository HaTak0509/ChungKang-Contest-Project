using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;

public class PullPunch : MonoBehaviour
{
    [Header("감지 설정")]
    [SerializeField] private Vector3 detectionPos = new Vector3(); // 감지할 사각형 위치
    [SerializeField] private Vector2 detectionSize = new Vector2(5f, 2f); // 감지할 사각형 크기
    [SerializeField] private LayerMask targetLayer;                     // 감지할 레이어 (Player 등)

    [Header("설정")]
    [SerializeField] private int maxResistance = 3;      // 탈출에 필요한 저항 횟수
    [SerializeField] private float resistCooldown = 0.2f; // 저항 입력 쿨타임
    [SerializeField] private float pullSpeed = 5.0f;      // 플레이어를 당기는 속도
    [SerializeField] private float thornSpeed = 15.0f;    // 가시 발사 속도
    [SerializeField] private float ReadyTime = 0.5f; //대기 시간
    [SerializeField] private float CoolTime = 1.5f; //쿨타임


    [Header("필수 연결")]
    [SerializeField] private GameObject _Niddle;      // 발사할 가시 프리팹
    [SerializeField] private Transform firePoint;         // 가시 발사구


    private bool _isPulling = false;
    private int _currentResistCount = 0;
    private float _lastResistTime = 0f;
    private Transform _caughtPlayer;

    void Update()
    {
        if(!_isPulling)
            CheckForTargets();

        if (_isPulling && _caughtPlayer != null)
        {
            HandleResistance();
            PullPlayer();
        }
    }

    // 내가 원할 때 호출하는 함수
    private async void CheckForTargets()
    {
        // 현재 위치에서 detectionSize만큼의 사각형 안에 있는 모든 콜라이더를 가져옴
        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(transform.position + detectionPos, detectionSize, 0f, targetLayer);

        if (hitTargets.Length > 0)
        {
            foreach (var target in hitTargets)
            {
                Debug.Log($"{target.name} 감지 성공!");

                if (!_isPulling)
                {
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
        await UniTask.WaitForSeconds(ReadyTime);

        FireThorn();
    }
    
    // 1. 플레이어 인식 시 가시 발사 (Trigger 등으로 호출)
    public async void FireThorn()
    {
        if (_Niddle != null || _isPulling) return;

        // 가시 생성
        _Niddle = Instantiate(_Niddle, firePoint.position, firePoint.rotation);

        // 가시가 날아갈 목표 지점 (현재 감지된 플레이어의 위치)
        // CheckForTargets에서 찾은 대상을 타겟으로 삼습니다.
        Vector3 targetPos = transform.position + detectionPos;

        // [아이디어] 가시가 목표까지 날아가는 과정
        bool hitPlayer = await MoveThornToTarget(_Niddle.transform, targetPos);

        if (hitPlayer)
        {
            Debug.Log("가시가 타겟에 명중함!");
        }
        else
        {
            await UniTask.WaitForSeconds(CoolTime);
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
        _isPulling = true;
        _currentResistCount = 0;

        // 플레이어의 이동을 잠시 멈춤 (플레이어 스크립트에 상태 제어 필요)

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
        _caughtPlayer.position = Vector3.MoveTowards(_caughtPlayer.position, transform.position, pullSpeed * Time.deltaTime);

        // 가시(사슬)도 플레이어 위치에 맞춰 업데이트
        if (_Niddle != null)
            _Niddle.transform.position = _caughtPlayer.position;
    }

    private void ReleasePlayer()
    {
        _isPulling = false;
        _caughtPlayer = null;
        if (_Niddle != null) Destroy(_Niddle);
        Debug.Log("플레이어를 놓아주었습니다.");
    }


    // 에디터에서 범위를 보기 위한 시각화
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + detectionPos, detectionSize);
    }

}