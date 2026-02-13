using UnityEngine;

public class Pushing : MonoBehaviour
{
    public bool isPushing;
    public bool pushing; //밀다
    public int pushingDirection;

    [SerializeField] AudioClip _audioSource;
    private GameObject _pushingAudio;
    private PlayerMovement _playrMovement;
    private PushingObject pushingObj;
    private TouchingDetection touchingDetection;
    private SpriteRenderer spr;
    private BoxCollider2D boxCol;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        _playrMovement = GetComponent<PlayerMovement>();
        touchingDetection = GetComponent<TouchingDetection>();
    }

    private void FixedUpdate()
    {
        if (pushing)
        {
            if (_pushingAudio == null)
            {
                _pushingAudio = new GameObject("Pushing");
                AudioSource source = _pushingAudio.AddComponent<AudioSource>(); // 원하는 스크립트나 컴포넌트 즉시 추가
                source.clip = _audioSource;
                source.loop = true;
                source.volume = 1f;
                source.outputAudioMixerGroup = SoundManager.Instance.SFXGroup;
                source.Play();

            }

        }
        else {
            Destroy( _pushingAudio );
        }
        



        if (pushingObj == null || !pushingObj.isActive)
        {
            isPushing = false;
            return;
        }

        if (!touchingDetection.IsGround)
        {
            Release();
            return;
        }

        isPushing = true;

        Vector2 playerVelX = _playrMovement.moveInput;

        if (Mathf.Abs(playerVelX.x) < 0.01f)
        {
            pushing = false;
            boxCol.enabled = true;
            pushingObj.Stop();
            return;
        }

        if (Mathf.Sign(playerVelX.x) == pushingDirection)
        {
            pushing = true;
            boxCol.enabled = false;
            pushingObj.CopyVelocity(playerVelX.x);
        }
        else
        {
            if (pushingDirection > 0)
            {
                spr.flipX = true;
            }
            else if (pushingDirection < 0)
            {
                spr.flipX = false;
            }
            Release();
        }
    }

    public void PushingDirection()
    {
        pushingDirection = ((int)Mathf.Sign(pushingObj.transform.position.x - transform.position.x));
        if (pushingDirection > 0)
        {
            spr.flipX = false;
        }
        else if (pushingDirection < 0)
        {
            spr.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PushingObject obj))
        {
            pushingObj = obj; // Range
            boxCol = collision.GetComponent<BoxCollider2D>(); // Range
        }
    }

    public void Release()
    {
        pushing = false;
        isPushing = false;

        if (boxCol != null)
            boxCol.enabled = true;

        if (pushingObj != null)
        {
            pushingObj.Stop();
            pushingObj.isActive = false;
        }
    }
}
    
