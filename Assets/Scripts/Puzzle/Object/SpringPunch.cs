using Cysharp.Threading.Tasks;
using UnityEngine;




public class SpringPunch : MonoBehaviour
{
    public Animator _animator;

    [Header("감지 설정")]
    [SerializeField] private Vector3 detectionPos = new Vector3(); // 감지할 사각형 위치
    [SerializeField] private Vector2 detectionSize = new Vector2(5f, 2f); // 감지할 사각형 크기
    [SerializeField] private LayerMask targetLayer;                     // 감지할 레이어 (Player 등)

    [Header("대기 시간")]
    [SerializeField] private float Force;
    [SerializeField] private float ReadyTime = 0.5f;
    [SerializeField] private float CoolTime = 1.5f;

    private bool _isActive = false;


    private void Start()
    {
    }

    private void Update()
    {
        if(!_isActive)
            CheckForTargets();
    }

    private async UniTask WaitPunch()
    {
        await UniTask.WaitForSeconds(ReadyTime);

        Punch();
    }

    async void Punch()
    {
        _animator.SetTrigger("Action");
        CheckForTargets();

        await CoolPunch();

    }

    private async UniTask CoolPunch()
    {
        await UniTask.WaitForSeconds(CoolTime);

        _animator.SetTrigger("End");

        // 1. 애니메이션이 시작될 때까지 한 프레임 대기
        await UniTask.Yield();

        // 2. 현재 재생 중인 애니메이션이 끝날 때까지 대기
        await UniTask.WaitUntil(() =>
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        _isActive = false;

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

                
                if (_isActive)
                {
                    target.GetComponent<Damageable>().TakePushFromPosition(transform.position, Force);
                }
                else{
                    _isActive = true;
                    await WaitPunch();
                }


            }
        }
        else
        {
            Debug.Log("범위 내에 아무도 없습니다.");
        }
    }

    // 에디터에서 범위를 보기 위한 시각화
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + detectionPos, detectionSize);
    }


}
