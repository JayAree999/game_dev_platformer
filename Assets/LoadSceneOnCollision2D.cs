using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnCollision2D : MonoBehaviour
{
    // Name of the scene to load
    public string sceneName = "L2";

    // Called when a 2D collision occurs
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has a tag of "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Load the specified scene
            SceneManager.LoadScene(sceneName);
        }
    }
}