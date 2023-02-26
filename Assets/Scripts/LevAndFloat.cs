using UnityEngine;
using UnityEngine.InputSystem;

public class LevAndFloat : MonoBehaviour
{
    
    private float startYPosition;

    public float levitateDistance = 5f;

    public float levitateDuration = 100f;
    public float levitateSpeed = 700f;
    public float floatSpeed = 100f;
    public float maxFloatTime = 100f;
    public float levitateCooldown = 0.7f;

    private bool isLevitating = false;
    private bool isFloating = false;
    private bool canJump = true;
    private float floatTime = 0f;
    private float levitateTimer = 0f;
    private float levitateCooldownTimer = 0f;

    private InputAction jumpAction;
    private InputAction levitateAction;
    private InputAction floatAction;

    private void Awake()
    {
        // Get references to the jump, levitate, and float actions
        jumpAction = new InputAction("Jump", binding: "<Keyboard>/space");
        levitateAction = new InputAction("Levitate", binding: "<Keyboard>/q");
        floatAction = new InputAction("Float", binding: "<Keyboard>/space");
        startYPosition = transform.position.y;
    }

    private void OnEnable()
    {
        // Enable the jump, levitate, and float actions
        jumpAction.Enable();
        levitateAction.Enable();
        floatAction.Enable();
    }

    private void OnDisable()
    {
        // Disable the jump, levitate, and float actions
        jumpAction.Disable();
        levitateAction.Disable();
        floatAction.Disable();
    }
    

    void Update()
    {
        // Check if levitation ability is on cooldown
        if (levitateCooldownTimer > 0f)
        {
            levitateCooldownTimer -= Time.deltaTime;
        }
        else
        {
            // Levitate upwards if the levitate action is triggered
            if (levitateAction.triggered && !isLevitating)
            {
                isLevitating = true;
                levitateTimer = 0f;
                levitateCooldownTimer = levitateCooldown;
                canJump = false; // Disable jumping while levitating
            }
        }

        // Levitate or float the player based on input
        if (isLevitating || isFloating)
        {
            float verticalMovement = 0f;

            if (isLevitating)
            {
                levitateTimer += Time.deltaTime;

                if ((levitateTimer <= levitateDuration) && (transform.position.y - startYPosition < levitateDistance))
                {
                    verticalMovement = levitateSpeed * Time.deltaTime;
                }
                else
                {
                    isLevitating = false;
                    canJump = true; // Enable jumping after levitating
                }
            }

            if (isFloating)
            {
                if (floatAction.ReadValue<float>() == 0f)
                {
                    isFloating = false;
                }
                else
                {
                    floatTime += Time.deltaTime;

                    if (floatTime <= maxFloatTime)
                    {
                        verticalMovement = -floatSpeed * Time.deltaTime;
                    }
                    else
                    {
                        isFloating = false;
                    }
                }
            }

            transform.Translate(new Vector3(0f, verticalMovement, 0f));
        }

        // Stop levitating or floating if the player lands on the ground
        if ((isLevitating || isFloating) && transform.position.y <= 0f)
        {
            isLevitating = false;
            isFloating = false;
            canJump = true; // Enable jumping after landing
        }
    }

    private void OnGroundCheck()
    {
        // Check if the player can jump and trigger the jump action
        if (canJump && jumpAction.triggered)
        {
            Debug.Log("Jump!");
        }
    }
}
           
