using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    public GameObject dropPrefab; // Prefab of the item to drop

    // Call this method to defeat the enemy and trigger the item drop
    public void Defeat()
    {
        // Instantiate the item at the enemy's position
        GameObject drop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        // Destroy the enemy object
        Destroy(gameObject);
    }
}