using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpingLaser : MonoBehaviour
{
    public bool active;
    
    private List<SpriteRenderer> puzzleObjects = new List<SpriteRenderer>();
    private SpriteRenderer player;

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
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("PuzzleObject"))
        {
            puzzleObjects.Add(collision.gameObject.GetComponent<SpriteRenderer>());
            Debug.Log(123);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("PuzzleObject"))
        {
            puzzleObjects.Remove(collision.gameObject.GetComponent<SpriteRenderer>());
            player = null;
        }
    }
}
