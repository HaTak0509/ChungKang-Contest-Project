using UnityEngine;

public class Crack : MonoBehaviour
{
    [SerializeField] private GameObject activationCrack;
    [SerializeField] private GameObject deactivationCrack;

    public bool activationCrackActive;
    public bool deactivationCrackActive;

    private bool _interactionActive;
    private bool _active;

    private void Awake()
    {
        activationCrackActive = false;
        deactivationCrackActive = false;
        _active = false;
    }

    private void Update()
    {
        if (deactivationCrackActive)
            deactivationCrack.SetActive(true);
        else
            deactivationCrack.SetActive(false);

        if (_interactionActive && !_active)
        {
            if (deactivationCrackActive)
            {
                if (Input.GetKey(KeyCode.F))
                {
                    activationCrackActive = true;
                    activationCrack.SetActive(true);
                    deactivationCrackActive = false;
                }
            }

            if (activationCrackActive)
            {
                if (Input.GetKey(KeyCode.F))
                {
                    activationCrackActive = false;
                    activationCrack.SetActive(false);
                    deactivationCrackActive = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _interactionActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _interactionActive = false;
        }
    }
}
