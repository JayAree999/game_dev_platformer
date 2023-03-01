using UnityEngine;

public class Fck : MonoBehaviour
{
    public float knockbackForce = 10f; // The force with which the player will be knocked back
    public float knockbackDuration = 0.5f; // The duration of the knockback effect
    private float knockbackTimer; // A timer to track the duration of the knockback effect

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Calculate the direction in which to knock the player back
            Vector3 knockbackDirection = collision.transform.position - transform.position;
            knockbackDirection.Normalize();

            // Apply the knockback force to the player
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

            // Start the knockback timer
            knockbackTimer = knockbackDuration;
        }
    }

    private void Update()
    {
        if (knockbackTimer > 0)
        {
            // Decrement the knockback timer
            knockbackTimer -= Time.deltaTime;

            // If the knockback timer has expired, stop the knockback effect
            if (knockbackTimer <= 0)
            {
                Rigidbody playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
                playerRigidbody.velocity = Vector3.zero;
            }
        }
    }
}