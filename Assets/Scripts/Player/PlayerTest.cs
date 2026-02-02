using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public Monster monster;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            monster.Warping();
        }
    }
}
