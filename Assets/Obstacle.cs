using UnityEngine;
public class Enemy : MonoBehaviour
{
    public float knockbackDistance = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Calculate the direction from the obstacle to the player
            Vector2 knockbackDirection = other.transform.position - transform.position;

            // Flip the direction if the player is facing the other way
            if ((other.transform.localScale.x > 0f && knockbackDirection.x < 0f) ||
                (other.transform.localScale.x < 0f && knockbackDirection.x > 0f))
            {
                knockbackDirection.x *= -1f;
            }

            // Keep the same height
            knockbackDirection.y = 0f;

            // Normalize the direction and apply the knockback force
            knockbackDirection.Normalize();
            Rigidbody2D playerRigidbody = other.gameObject.GetComponent<Rigidbody2D>();
            playerRigidbody.velocity = knockbackDirection * knockbackDistance;
        }
    }
}