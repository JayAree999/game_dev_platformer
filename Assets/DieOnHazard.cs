using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class DieOnHazard : MonoBehaviour
{
    // This method is called when the object collides with another object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision was with a specific tag
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.Die();
            }
            // Perform some action, for example, destroy the object
        }

        Destroy(gameObject);
    }
}