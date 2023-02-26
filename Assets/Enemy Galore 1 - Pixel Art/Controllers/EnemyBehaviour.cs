using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float detectionDistance = 10f; // Distance at which the enemy detects the player
    public float attackDistance = 2f; // Distance at which the enemy attacks the player
    public float damage = 10f; // Damage dealt by the enemy

    private Transform player; // Reference to the player's transform
    private Animator animator; // Reference to the animator component

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the player is within attack distance, attack the player
        if (distanceToPlayer <= attackDistance)
        {
            // Trigger the attack animation
            animator.SetTrigger("Attack 3");
        }
    }

    // Called by the animation event at the end of the attack animation
    public void ResetAttackTrigger()
    {
        animator.ResetTrigger("Attack 3");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the enemy collides with the player, stop moving
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Run", false);
        }
    }
}