using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1.4f;
    public float jumpForce = 2f;

    public float fallMultiplier = 1.5f;
    public float levitateSpeed = 2.7f;
    public float levitateDuration = 0.1f;
    public float levitateCooldown = 0.5f;

    private bool isLevitating;
    private float lastLevitateTime = -100f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool isGrounded;
    private bool isDashing;
    private bool hasJumped;
    private float lastDashTime = -100f;
    private float dashDistanceTraveled2 = 0f;

    public float dashDistance = 1.125f;
    public float dashCooldown = 0.5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        HandleMovement();
        HandleFlipping();
        CheckGrounded();
        HandleJumping();
        HandleInstantDash();
        HandleFloating();
        HandleLevitate();
        if (isGrounded)
        {
            hasJumped = false;
        }
    }

    void CheckGrounded()
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCollider.bounds.center, boxCollider.bounds.size, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.tag == "Ground")
            {
                isGrounded = true;
                break;
            }
        }
    }
    private Animator animator;
    void HandleMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        animator.SetFloat("Horizontal", Mathf.Abs(horizontalMovement));
    }

    void HandleFloating()
    {
        if (Input.GetKey(KeyCode.Space) && !isGrounded && !isLevitating) {
            rb.gravityScale = 0.5f;
        } else {
            rb.gravityScale = 1f;
        }
    }

    void FixedUpdate()
    {
        HandleFloating();
        HandleLevitate();
    }

    void HandleJumping()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            hasJumped = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (hasJumped && (Input.GetKeyUp(KeyCode.Space) || rb.velocity.y < 0))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void HandleInstantDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - lastDashTime > dashCooldown)
        {
            lastDashTime = Time.time;
            dashDistanceTraveled2 = 0f;
            Vector2 dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            StartCoroutine(PerformDash(dashDirection));
        }
    }

    IEnumerator PerformDash(Vector2 direction)
    {
        while (dashDistanceTraveled2 < dashDistance)
        {
            float dashDistanceLeft = dashDistance - dashDistanceTraveled2;
            float dashDistanceThisFrame = Mathf.Min(dashDistanceLeft, Time.deltaTime * moveSpeed * 40f);
            rb.position += direction * dashDistanceThisFrame;
            dashDistanceTraveled2 += dashDistanceThisFrame;
            yield return null;
        }
    }



    

    void HandleFlipping()
    {
        Vector3 localScale = transform.localScale;

        if (rb.velocity.x > 0)
        {
            localScale.x = 1;
        }
        else if (rb.velocity.x < 0)
        {
            localScale.x = -1;
        }

        transform.localScale = localScale;
    }

    void HandleLevitate()
    {
        if (Time.time - lastLevitateTime > levitateCooldown)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isDashing && !isLevitating)
            {
                isLevitating = true;
                lastLevitateTime = Time.time;
                StartCoroutine(Levitate());
            }
        }
    }


    IEnumerator Levitate()
    {
        float startTime = Time.time;
        while (Time.time - startTime < levitateDuration)
        {
            rb.velocity = new Vector2(rb.velocity.x, levitateSpeed);
            yield return null;
        }

        isLevitating = false;
    }
}


