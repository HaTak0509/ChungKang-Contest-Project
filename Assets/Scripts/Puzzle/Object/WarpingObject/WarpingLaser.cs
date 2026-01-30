using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class WarpingLaser : MonoBehaviour
{
    public bool active;
    
    private List<SpriteRenderer> puzzleObjects = new List<SpriteRenderer>();
    private PlayerColor player;

    private void Awake()
    {
        active = false;
    }

    private void Update()
    {
        if (!active) return;

        foreach (var puzzleSp in puzzleObjects)
        {
            Color c = puzzleSp.color;
            c.a = 0.3f;
            puzzleSp.color = c;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerColor>();
            player.LowerTransparency().Forget();
        }
        else if (collision.gameObject.CompareTag("PuzzleObject"))
        {
            puzzleObjects.Add(collision.gameObject.GetComponent<SpriteRenderer>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.UpperTransparency().Forget();
            player = null;
        }
        else if (collision.gameObject.CompareTag("PuzzleObject"))
        {
            puzzleObjects.Remove(collision.gameObject.GetComponent<SpriteRenderer>());

        }
    }
}
