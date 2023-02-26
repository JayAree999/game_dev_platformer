using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KeyManager : MonoBehaviour
{
    public GameObject gameObj;     // The game object to make visible
    public string nextScene;       // The name of the scene to load when the keys are found
    public Text keyCountText;      // The UI text to display the key count

    private int keyCount = 0;      // The number of keys that the player has found
    void Start()
    {
        gameObj.SetActive(false); // Set the game object to be inactive at the start of the game
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        Debug.Log("Destroying key object");
        if (other.CompareTag("Key"))  // Check if the collider belongs to a key object
        {
            Destroy(other.gameObject);  // Destroy the key object
            keyCount++;                  // Increment the key count

            if (keyCount >= 3)           // Check if the player has found all 3 keys
            {
                gameObj.SetActive(true);  // Make the game object visible
            }
        }
        
       
    }
    
   


    void Update()
    {
        keyCountText.text = "Keys Found: " + keyCount + "/3";  // Update the UI text with the current key count
    }

}