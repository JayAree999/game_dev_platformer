using UnityEngine;

public class DestroyOnCollide : MonoBehaviour
{
    private int coinValue = 10;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            FindObjectOfType<GameSession>().AddToScore(coinValue);
            Destroy(gameObject);
        }
    }
}