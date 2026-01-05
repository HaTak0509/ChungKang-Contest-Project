using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelReset : MonoBehaviour
{
    public static LevelReset Instance;

    public bool reset;
    private GameObject currentScene;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        currentScene = GetComponent<GameObject>();
    }

    private void Update()
    {
        if (reset)
        {
            OnReset();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            reset = true;
        }
    }

    private void OnReset()
    {
        reset = false;
        Instantiate(currentScene);
    }
}
