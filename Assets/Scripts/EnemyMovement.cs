using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D rgbd2D;

    void Start()
    {
        rgbd2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rgbd2D.velocity = new Vector2 (moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing() 
    {
        Vector3 newScale = transform.localScale;
        newScale.x = -(Mathf.Sign(rgbd2D.velocity.x)) * 5f;
        newScale.y = 5f;
        newScale.z = 5f;
        transform.localScale = newScale;
    }

}
