using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Game Objects References")]

    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerObject;
    [SerializeField] private ThirdPersonCamera thirdPersonCamera;
    [SerializeField] private LayerMask gateLayerMask;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject stepRayUpper;
    [SerializeField] private GameObject stepRayLower;

    [Header("Player Settings")]

    [SerializeField] private float playerHeight = 1.75f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpCooldown = 1.1f;
    [SerializeField] private float airMultiplier = 0.5f;
    [SerializeField] private float stepHeight = 0.3f;
    [SerializeField] private float stepSmooth = 0.1f;
    [SerializeField] private float groundDrag = 2f;

    [Header("Player State")]

    private Rigidbody rigitBody;

    private Vector3 lastInteractDirection;

    private float playerSpeed;

    private bool isWalking;
    private bool isRunning;
    private bool isGrounded;
    private bool hasJumped;
    private bool isInAir;
    private bool isReadyToJump = true;

    private void Awake()
    {
        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);
        playerSpeed = moveSpeed;
    }

    private void Start()
    {
        rigitBody = GetComponent<Rigidbody>();
        rigitBody.freezeRotation = true;
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnRunAction += GameInput_OnRunAction;
        gameInput.OnJumpAction += GameInput_OnJumpAction;
    }

    private void Update()
    {
        IsGrounded();
        HandleMovement();
        HandleDrag();
        SpeedControl();
        //HandleStairs();
    }

    private void HandleMovement()
    {
        UpdateOrientation();

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 directionVector = orientation.forward * inputVector.y + orientation.right * inputVector.x;

        float moveDistance = playerSpeed * Time.deltaTime;
        float playerRadius = 0.3f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionVector, moveDistance);

        if (!canMove)
        {
            Vector3 directionVectorX = new Vector3(directionVector.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionVectorX, moveDistance);
            if (canMove)
            {
                directionVector = directionVectorX;
            }
            else
            {
                Vector3 directionVectorZ = new Vector3(0, 0, directionVector.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionVectorZ, moveDistance);
                if (canMove)
                {
                    directionVector = directionVectorZ;
                }
            }
        }

        if (canMove)
        {
            float speedMultiplier = 10f;
            if (isGrounded)
            {
                rigitBody.AddForce(directionVector * playerSpeed * speedMultiplier, ForceMode.Force);
            }
            else
            {
                rigitBody.AddForce(directionVector * playerSpeed * speedMultiplier * airMultiplier, ForceMode.Force);
            }
        }

        float rotationSpeed = 10f;

        if (directionVector != Vector3.zero)
        {
            playerObject.forward = Vector3.Slerp(playerObject.forward, directionVector.normalized, Time.deltaTime * rotationSpeed);
        }

        isWalking = inputVector != Vector2.zero;
        isRunning = isRunning && inputVector != Vector2.zero;
    }

    private void UpdateOrientation()
    {
        Vector3 thirdPersonCameraPosition = thirdPersonCamera.transform.position;
        orientation.forward = (transform.position - new Vector3(thirdPersonCameraPosition.x, transform.position.y, thirdPersonCameraPosition.z)).normalized;
    }

    private void HandleStairs()
    {
        float stepDistance = 1f;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitLower, stepDistance))
        {
            Debug.Log("Lower hit");
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitUpper, stepDistance + 0.1f))
            {
                Debug.Log("Upper did not hit");
                transform.position += new Vector3(0, +stepHeight, 0);
            }
        }
    }

    private void HandleDrag()
    {
        if (isGrounded)
        {
            rigitBody.linearDamping = groundDrag;
        }
        else
        {
            rigitBody.linearDamping = 0;
        }
    }

    private void Jump()
    {
        rigitBody.angularVelocity = new Vector3(rigitBody.angularVelocity.x, 0, rigitBody.angularVelocity.z);
        rigitBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        isReadyToJump = true;
        hasJumped = false;
    }

    private void GameInput_OnRunAction(object sender, System.EventArgs e)
    {
        isRunning = !isRunning && isWalking;
        playerSpeed = isRunning ? runSpeed : moveSpeed;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 directionVector = orientation.forward * inputVector.y + orientation.right * inputVector.x;

        if (directionVector != Vector3.zero)
        {
            lastInteractDirection = directionVector;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, gateLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out MainGate mainGate))
            {
                mainGate.Interact();
            }
        }
    }

    private void GameInput_OnJumpAction(object sender, System.EventArgs e)
    {
        if (isReadyToJump && isGrounded)
        {
            hasJumped = true;
            isReadyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rigitBody.linearVelocity.x, 0, rigitBody.linearVelocity.z);
        if (flatVelocity.magnitude > playerSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * playerSpeed;
            rigitBody.linearVelocity = new Vector3(limitedVelocity.x, rigitBody.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void IsGrounded()
    {
        float raycastDistance = 0.2f;
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, raycastDistance, groundMask);
    }

    public bool IsInAir()
    {
        if (isGrounded && isReadyToJump)
        {
            isInAir = false;
        }
        else
        {
            isInAir = true;
        }
        return isInAir;
    }

    public bool HasJumped()
    {
        return hasJumped;
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public bool IsRunning()
    {
        return isRunning;
    }
}
