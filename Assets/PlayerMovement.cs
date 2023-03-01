using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Movement variables
    public float baseMoveSpeed = 5f;
    public float moveSpeed;
    bool isAlive = true;
    [SerializeField] float movementSpeed = 10f;
    
    private float fastTimeRemaining = 0f;
    private float fastMultiplier = 0f;
    public float reducedDelay = 0.5f;
    public float delayReductionDuration = 5f;

    public float shootDelay = 0.5f;

    private float nextFireTime = 0f;
    public GameSession gameSession;
    private float shootDelayReductionDuration = 0f;
    private float manaFullTime;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isGrounded;

    // Dash variables
    public float dashDistance = 3f;
    public float dashDuration = 0.2f;
    public float bulletVelocity = 15f;
    public float dashCooldown = 1f;
    private float nextDashTime;

    // Shooting variables
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    // Animation variables
    private Animator animator;
    public int facingDirection = 2;

    // Dying variables
    private bool isMovingFast = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameSession = FindObjectOfType<GameSession>();
        moveSpeed = baseMoveSpeed;
        nextDashTime = Time.time;
        manaFullTime = -delayReductionDuration; 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            animator.SetBool("isClimbing", true);
            isClimbing = true;
            
            rb.gravityScale = 0f; // disable gravity while climbing
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            animator.SetBool("isClimbing", false);
            isClimbing = false;
            rb.gravityScale = 1f; // enable gravity after climbing
        }
    }
    public void StartFastMovement(float duration, float multiplier)
    {
        if (!isMovingFast)
        {
            isMovingFast = true;
            fastTimeRemaining = duration;
            fastMultiplier = multiplier;
            movementSpeed *= fastMultiplier;
        }
    }


    void Update()
    {
        if (shootingTimer > 0)
        {
            shootingTimer -= Time.deltaTime;
        }
        if (isMovingFast)
        {
            fastTimeRemaining -= Time.deltaTime;

            if (fastTimeRemaining <= 0f)
            {
                isMovingFast = false;
                movementSpeed /= fastMultiplier;
            }
        }


        if (gameSession.manaCount > 6)
        {
            movementSpeed *= 1.5f;
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();

        }
        if (isClimbing)
        {
            
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector2 climbVelocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed);
            rb.velocity = climbVelocity;
        }
       
        if(!isAlive) { return; };
        // Update the isRunning parameter based on the player's movement
        animator.SetBool("isRunning", Mathf.Abs(rb.velocity.x) > 0.1f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }

        if (Input.GetButton("Fire1"))
        {

            Shoot();
        }
      

    }

    void FixedUpdate()
    {
        float moveDirection = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if (moveDirection != 0)
        {
            facingDirection = (int)Mathf.Sign(moveDirection);
            transform.localScale = new Vector3(facingDirection * 2f, 2f, 2f);

        }
    }

    void Jump()
    {
        // Jump only if the player is on the ground
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;

            // Trigger the jumping animation
            animator.SetBool("isJumping", true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player lands on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }

        // Check if the player collides with an enemy
      
    }

    [SerializeField] Vector2 deathKick = new Vector2(0,2f);
    public void Die()
    {
        Debug.Log("DIE");
        // Instantiate the death effect and destroy the player object
        animator.SetTrigger("Dying");
        
        rb.velocity = deathKick;
        float delay = 100f;
        Destroy(gameObject, delay);

        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
  
   
    void Dash()
    {
        if (Time.time > nextDashTime)
        {
            // Set the dash destination based on the player's facing direction
            Vector2 dashDestination = transform.position + new Vector3(facingDirection * dashDistance, 0f);

            // Move the player to the dash destination
            transform.position = dashDestination;

            // Set the next dash time and disable movement temporarily
            nextDashTime = Time.time + dashCooldown;
            moveSpeed = 0f;

            // Trigger the dashing animation
            animator.SetBool("isDashing", true);

            // Enable movement again after the dash duration
            Invoke("EnableMovement", dashDuration);
        }
    }
    
    

    void EnableMovement()
    {
        moveSpeed = baseMoveSpeed;
        animator.SetBool("isDashing", false);
    }
    [SerializeField] private float climbSpeed = 3f; // speed at which the player climbs

    private bool isClimbing = false;

    public GameObject originalBulletPrefab;
    public GameObject fastBulletPrefab;
    private float nextShootTime;

    public float shootingCooldown = 0.5f;
    private float shootingTimer = 0f;

    public float fastShootDelay = 0.2f;
    public bool isFast = false;

    private void Shoot()
    {
        
        if (Time.time > nextShootTime)
        {
            GameObject bullet;
            if (isFast)
            {
                animator.SetTrigger("Knifeing");
                bullet = Instantiate(fastBulletPrefab);
                nextShootTime = Time.time + fastShootDelay;
            }
            else
            {
                animator.SetTrigger("Shooting");
                bullet = Instantiate(bulletPrefab);
                nextShootTime = Time.time + shootDelay;
            }
        
            bullet.transform.position = transform.position + new Vector3(0.5f, 0f, 0f);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (facingDirection == -1)
            {
                bullet.transform.localScale = new Vector3(-1f, 1f, 1f);
                rb.velocity = new Vector2(-10f, 0f);
            }
            else
            { 
                rb.velocity = new Vector2(10f, 0f);
            }
        }
    }


    

  
    
    




    




}
    
