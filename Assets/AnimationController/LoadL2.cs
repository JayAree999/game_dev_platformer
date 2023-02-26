using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadL2 : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))   // Check if the collider belongs to the player object
        {
            SceneManager.LoadScene("L2");         // Load the next scene
        }
    }
}