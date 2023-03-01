using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2.0f;
    private bool movingRight = true;

    void Update()
    {
        // Move the enemy back and forth between point A and point B
        if (movingRight)
        {
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            if (transform.position.x >= pointB.position.x)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
            if (transform.position.x <= pointA.position.x)
            {
                movingRight = true;
            }
        }
    }
}