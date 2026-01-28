using UnityEngine;

public class Crack : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject activationCrack;
    [SerializeField] private GameObject deactivationCrack;
    [SerializeField] private GameObject closeCrack;

    public int _openLimit;

    private bool _isActivated;
    private bool _playerInRange;
    private int _currentLimit;

    private void Awake()
    {
        SetCrack(false);
        deactivationCrack.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("ExplorationRange")) return;
        _playerInRange = true;

        if (!_isActivated)
            deactivationCrack.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("ExplorationRange")) return;
        _playerInRange = false;
        deactivationCrack.SetActive(false);
    }

    public void Interact()
    {
        if (!_playerInRange) return;

        SetCrack(!_isActivated);
    }

    private void SetCrack(bool value)
    {
        if (_currentLimit >= _openLimit) return;

        _isActivated = value;

        activationCrack.SetActive(_isActivated);
        deactivationCrack.SetActive(!_isActivated && _playerInRange);
        closeCrack.SetActive(_isActivated && _playerInRange);
    }

    public void SetPlayerInRange(bool value)
    {
        _playerInRange = value;

        // 아직 활성화 안 된 상태면 표시 제어
        if (!_isActivated)
        {
            deactivationCrack.SetActive(_playerInRange);
        }
    }
}
