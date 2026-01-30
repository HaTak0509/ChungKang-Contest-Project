using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public Monster monster;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            monster.TwistMob();
        }

    }
}
