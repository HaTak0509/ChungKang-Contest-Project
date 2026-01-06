using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private int nextLevel;

    public bool clear;

    private bool _action;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _action)
        {
            Debug.Log(123);

            // LevelReset.Instance.LoadLevel(nextLevel);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && clear)
            _action = true;
    }
    private void OnTriggerExitD(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            _action = false;
    }
}
