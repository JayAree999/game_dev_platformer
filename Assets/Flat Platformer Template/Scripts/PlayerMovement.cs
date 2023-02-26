using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] float fallMultiplier = 2.5f; // new variable to increase gravity while falling
    [SerializeField] float lowJumpMultiplier = 2f; // new variable to decrease jump height
    [SerializeField] float dashDistance = 15f;
    [SerializeField] float dashDuration = 0.1f;
    [SerializeField] float dashCooldown = 0.1f;
    bool isDashing = false;
    float dashTimer = 0.0f;
    float dashCooldownTimer = 0.0f;
    [SerializeField] float runSpeed = 7.5f;
    [SerializeField] float climbSpeed = 2.0f;
    [SerializeField] float jumpForce = 7f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    Vector2 moveInput;
    bool jumpInput;
    Rigidbody2D rgbd2D;
    bool isGrounded = false;   
    bool isJumping = false; // new bool to track jumping state
	bool isAlive = true;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    CircleCollider2D myCircleCollider;
    float gravityScaleAtStart;

    void Start()
    {
        rgbd2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myCircleCollider = GetComponent<CircleCollider2D>();
        gravityScaleAtStart = rgbd2D.gravityScale;
        myAnimator.SetBool("isJumping", false);
    }

    void Update()
    {

   		if(!isAlive) { return; };
        Run();
        FlipSprite();
        ClimbLadder();
        Jump();
        Dash();
		Die();


		
        
        // Apply gravity to the player based on whether they are falling or jumping
        if (rgbd2D.velocity.y < 0) // player is falling
        {
            rgbd2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rgbd2D.velocity.y > 0 && !Keyboard.current.spaceKey.isPressed) // player is jumping but not holding down the jump key
        {
            rgbd2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
		
    
    [SerializeField] float dashSpeed = 12.5f; // added a new dash speed of 12.5

	void OnFire(InputValue value)
    	{
        	if(!isAlive) { return; };
        	Instantiate(bullet, gun.position, transform.rotation);
        	myAnimator.SetTrigger("Shooting");

    	}
    
    void Dash()
    {
		if(!isAlive) { return; };
        // If the player presses the left shift key and the dash is not on cooldown
        if(Keyboard.current.leftShiftKey.wasPressedThisFrame && dashCooldownTimer <= 0)
        {
            // Start the dash
            isDashing = true;
            dashTimer = 0.0f;

            // Set the player's velocity to the dash distance divided by the dash duration
            Vector2 dashVelocity = new Vector2(transform.localScale.x * dashDistance / dashDuration, rgbd2D.velocity.y);
            rgbd2D.velocity = dashVelocity;

            // Disable gravity while dashing
            rgbd2D.gravityScale = 0.0f;
        }

        // If the player is dashing, add to the dash timer and check if it's done
        if(isDashing)
        {
            dashTimer += Time.deltaTime;

            // If the dash timer is up, end the dash
            if(dashTimer >= dashDuration)
            {
                isDashing = false;

                // Re-enable gravity
                rgbd2D.gravityScale = gravityScaleAtStart;

                // Start the dash cooldown timer
                dashCooldownTimer = dashCooldown;
            }
        }

        // If the dash is on cooldown, add to the cooldown timer
        if(dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }   
    }

    void OnMove(InputValue value)
    {
		if(!isAlive) { return; };
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
		if(!isAlive) { return; };
        jumpInput = value.Get<float>() > 0.0f;
    }

    void Run()
    {
		if(!isAlive) { return; };
        Vector2 playerVelocity = new Vector2 (moveInput.x*runSpeed , rgbd2D.velocity.y);
        rgbd2D.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rgbd2D.velocity.x) >  Mathf.Epsilon;
        if(playerHasHorizontalSpeed) 
        {
            myAnimator.SetBool("isRunning", true);
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    void FlipSprite()
    {
		if(!isAlive) { return; };
        bool playHasHorizontalSpeed = Mathf.Abs(rgbd2D.velocity.x) > Mathf.Epsilon; 
        if(playHasHorizontalSpeed) 
        {
            transform.localScale = new Vector2 (Mathf.Sign(rgbd2D.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    void ClimbLadder()
    {
		if(!isAlive) { return; };
        // if not touching the ladder
        if(!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) 
        {
            rgbd2D.gravityScale = gravityScaleAtStart; 
            myAnimator.SetBool("isClimbing", false);
            return; 
        }

        Vector2 climbVelocity = new Vector2 (rgbd2D.velocity.x,moveInput.y*climbSpeed);
        rgbd2D.velocity = climbVelocity;
        rgbd2D.gravityScale = 0f;

        // check vertical Speed
        bool playHasVerticalSpeed = Mathf.Abs(rgbd2D.velocity.y) > Mathf.Epsilon; 

        myAnimator.SetBool("isClimbing", playHasVerticalSpeed);
        myAnimator.SetBool("isJumping", false);
        
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }
    
    void Jump()
    {
        if(isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpForce);
            rgbd2D.velocity += jumpVelocityToAdd;
            isGrounded = false;

            // Set the "isJumping" parameter to true when the player jumps
            isJumping = true;
            myAnimator.SetBool("isJumping", isJumping);
        }

        // Reset the "isJumping" parameter to false when the player lands
        if(isGrounded && isJumping)
        {
            isJumping = false;
            myAnimator.SetBool("isJumping", isJumping);
        }
    }
	[SerializeField] Vector2 deathKick = new Vector2(0,2f);
	void Die() 
    {
        if(myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            rgbd2D.velocity = deathKick;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }    
    }
	
}

