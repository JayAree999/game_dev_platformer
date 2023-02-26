using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision was with the player
        PlayerHealthUI playerHealth = collision.gameObject.GetComponent<PlayerHealthUI>();
        if (playerHealth != null)
        {
            // If the collision was with the player, decrease their health
            playerHealth.TakeDamage(damageAmount);
        }
    }
    [SerializeField] int maxHealth = 3;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}