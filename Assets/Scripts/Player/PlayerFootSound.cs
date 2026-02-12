using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootSound : MonoBehaviour
{
    // Start is called before the first frame update
    public void WalkSound()
    {
        SoundManager.Instance.PlaySFX("player_move", SoundManager.SoundOutput.SFX, 0.6f, Random.Range(0.75f, 1.0f));
    }

}
