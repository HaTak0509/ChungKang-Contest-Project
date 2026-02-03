using UnityEngine;

public class Crack : MonoBehaviour
{
    [SerializeField] private GameObject activationCrack;
    [SerializeField] private GameObject deactivationCrack;
    [SerializeField] private GameObject closeCrack;

    public int openLimit;
    public bool isActivated;

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
        if (_currentLimit >= openLimit) return;
        
        isActivated = value;
        Debug.Log(123);
        activationCrack.SetActive(isActivated);
        deactivationCrack.SetActive(!isActivated && _playerInRange);
        closeCrack.SetActive(isActivated && _playerInRange);
    }

    public void SetPlayerInRange(bool value)
    {
        _playerInRange = value;

        // 아직 활성화 안 된 상태면 표시 제어
        if (!isActivated)
        {
            deactivationCrack.SetActive(_playerInRange);
        }
    }
}
