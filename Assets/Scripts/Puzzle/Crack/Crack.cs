using UnityEngine;

public class Crack : MonoBehaviour
{
    [SerializeField] private GameObject activationCrack;
    [SerializeField] private GameObject deactivationCrack;
    [SerializeField] private GameObject closeCrack;

    public bool isActivated;

    private bool _playerInRange;

    private void Awake()
    {
        isActivated = false;

        activationCrack.SetActive(false);
        deactivationCrack.SetActive(false);
        closeCrack.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("ExplorationRange")) return;
        _playerInRange = true;

        if (!isActivated)
            deactivationCrack.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("ExplorationRange")) return;
        _playerInRange = false;
        deactivationCrack.SetActive(false);
    }

    public void SetCrack(bool value)
    {
        if (isActivated == value) return; // 상태 변화 없으면 무시

        if (CrackManager.Instance == null) return;

        SoundManager.Instance.PlaySFX("crack_create", SoundManager.SoundOutput.SFX, 1);

        if (value) // false → true
        {
            if (CrackManager.Instance.currentLimit >= CrackManager.Instance.crakLimit)
                return;

            CrackManager.Instance.currentLimit++;
        }
        else // true → false
        {
            CrackManager.Instance.currentLimit--;
        }

        isActivated = value;

        if (activationCrack != null)
            activationCrack.SetActive(isActivated);

        if (deactivationCrack != null)
            deactivationCrack.SetActive(!isActivated && _playerInRange);

        if (closeCrack != null)
            closeCrack.SetActive(isActivated && _playerInRange);

    }

    public void SetPlayerInRange(bool value)
    {
        _playerInRange = value;

        if (!isActivated)
        {
            deactivationCrack.SetActive(_playerInRange);
        }
    }
}
