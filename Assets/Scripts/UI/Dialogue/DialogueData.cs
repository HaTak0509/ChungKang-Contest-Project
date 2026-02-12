
using UnityEngine;

public class DialogueData : MonoBehaviour
{


        [SerializeField] private KeyCode targetKey = KeyCode.E;

        [TextArea(2, 5)]
        public string lines; // 해당 상태의 대사
     private bool _enable = false;

    private void OnTriggerEnter2D(Collider2D collision)
     {
        if (collision.transform.CompareTag("Player") && !_enable)
        {

            DialogueManager.Instance.UpdateFormattedText(lines,targetKey);
            _enable = true;
        }
     }
}