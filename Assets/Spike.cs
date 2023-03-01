using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public int scoreValue = 6; // Score value to add when the object is destroyed
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Add score to the game session
            FindObjectOfType<GameSession>().AddToScore(scoreValue);
            Destroy(gameObject);

        }
    }
}
