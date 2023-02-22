using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1.75f; // speed of player movement
    public float jumpForce = 2.5f; // force applied when jumping
    public float secondJumpForce = 5f; // additional force applied on second jump
    public float secondJumpAngle = 90f; // angle at which the second jump force is applied
    private Rigidbody2D rb; // reference to the player's rigidbody
    private int numJumps; // number of jumps the player has performed

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        numJumps = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // horizontal movement
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (numJumps == 0)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
            else if (numJumps == 1)
            {
                Vector2 jumpDirection = new Vector2(Mathf.Sign(rb.velocity.x), 1f).normalized;
                Vector2 jumpForceVector = jumpDirection * secondJumpForce;
                rb.velocity = new Vector2(0f, 0f);
                rb.AddForce(jumpForceVector, ForceMode2D.Impulse);
            }

            numJumps++;
        }
    }

    // Reset the number of jumps when the player touches the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            numJumps = 0;
        }
    }
}