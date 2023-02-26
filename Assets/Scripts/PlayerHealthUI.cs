using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerHealthUI : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth = 3;

    public Text healthText;
    public Vector3 offset = new Vector3(0, 1, 0);

    public GameObject dyingAnimationPrefab;

    void Start()
    {
        // Get the game state manager object
        GameObject gameStateManagerObject = GameObject.FindGameObjectWithTag("GameStateManager");

        // If the game state manager object exists, set the player's current health to the saved health value
        if (gameStateManagerObject != null)
        {
            GameStateManager gameStateManager = gameStateManagerObject.GetComponent<GameStateManager>();
            if (gameStateManager != null && gameStateManager.savedHealth != -1)
            {
                currentHealth = gameStateManager.savedHealth;
            }
        }
        else
        {
            // If the game state manager object does not exist, set the current health to the maximum health
            currentHealth = maxHealth;
        }
    }

    void Update()
    {
        // Update the text display to show the current health
        healthText.text = "Health: " + currentHealth.ToString();

        // Position the text display above the player's gameObject
        healthText.transform.position = Camera.main.WorldToScreenPoint(transform.position + offset);

        // If the player's health is below maxHealth, play the dying animation and reload the scene
        if (currentHealth <= 0)
        {
            // Instantiate the dying animation prefab at the player's position and rotation
            GameObject dyingAnimation = Instantiate(dyingAnimationPrefab, transform.position, transform.rotation);

            // Trigger the dying animation
            Animator animator = dyingAnimation.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Dying");
            }

            // Destroy the player gameObject
            Destroy(gameObject);
            
            SceneManager.LoadScene("Dead");
        }
    }

    // This method can be called to decrease the player's health
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // Update the GameStateManager with the current health value
        GameStateManager gameStateManager = FindObjectOfType<GameStateManager>();
        if (gameStateManager != null)
        {
            gameStateManager.savedHealth = currentHealth;
        }
    }

}
