using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public int savedHealth = -1;

    private void Awake()
    {
        // Ensure that there is only one instance of the game state manager
        if (FindObjectsOfType<GameStateManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDisable()
    {
        // Reset the saved health value when the game state manager is disabled
        savedHealth = -1;
    }
}