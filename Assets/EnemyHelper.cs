using UnityEngine;

public class EnemyHelper : MonoBehaviour
{
    public float attackDistance = 1.0f; // Distance at which the enemy attacks the player
    public float knockbackDistance = 2.0f; // Distance the player is knocked back when attacked
    public float knockbackDuration = 0.5f; // Duration of the knockback effect
    private Transform target; // Reference to the player GameObject
    private bool isAttacking = false; // Flag to prevent multiple attacks in a single frame
    private PlayerStats _playerStats;
   
    void Start()
    {
        // Find the player GameObject based on its "Player" tag
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    

    void Update()
    {
        // Attack the player if they are within the attack distance
        if (target != null && Vector2.Distance(transform.position, target.position) < attackDistance && !isAttacking)
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(2f);
            }

           
            
           
            
            
            
            // Move the player backwards
            Vector2 knockbackDirection = (target.position - transform.position).normalized;
            target.position += new Vector3(knockbackDirection.x, knockbackDirection.y, 0) * knockbackDistance;

            // Set a flag to prevent multiple attacks in a single frame
            isAttacking = true;
            Invoke("ResetAttackFlag", knockbackDuration);
        }
    }

    // Resets the isAttacking flag after the knockbackDuration has elapsed
    void ResetAttackFlag()
    {
        isAttacking = false;
    }
}