using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10; // Maximum health of the enemy
    private int currentHealth; // Current health of the enemy

    void Start()
    {
        // Initialize the enemy's health to the maximum value
        currentHealth = maxHealth;
    }

    // This method is called when the enemy is hit by a bullet
    public void TakeDamage(int damage)
    {
        // Subtract the damage from the enemy's health
        currentHealth -= damage;

        // If the enemy's health is less than or equal to 0, defeat the enemy
        if (currentHealth <= 0)
        {
            GetComponent<EnemyDestroy>().Defeat();
        }
    }

    // This method is called when the enemy collides with another object
    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the enemy collides with an object with the "Bullet" tag, take damage
        if (collision.gameObject.CompareTag("Bullet"))
        {
            
            // Call the TakeDamage method with the bullet's damage value
            TakeDamage(1);

            // Destroy the bullet object
            Destroy(collision.gameObject);
        }
    }
}