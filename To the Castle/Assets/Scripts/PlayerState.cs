using UnityEngine;
using UnityEngine.Playables;

public class PlayerState : MonoBehaviour, IEntityState
{
    [Header("Hierarchy References")]

    private EntityRigidBody entityRigidBody;

    [Header("Player Settings")]

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpCooldown = 1.2f;

    [Header("Player State")]

    private bool isWalking;
    private bool isRunning;
    private bool isJumping;
    private bool isGrounded;
    private bool hasJumped;
    private bool isInAir;
    private bool isReadyToJump;

    private void Awake()
    {
        entityRigidBody = GetComponent<EntityRigidBody>();
        entityRigidBody.EntitySpeed = moveSpeed;

        isReadyToJump = true;
    }

    private void Update()
    {
        isGrounded = entityRigidBody.IsGrounded();
        IsInAir();
    }

    public bool IsWalking
    {
        get => isWalking;
        private set => isWalking = value;
    }

    public bool IsRunning
    {
        get => isRunning;
        private set => isRunning = value;
    }

    public bool IsJumping
    {
        get => isJumping;
        private set => isJumping = value;
    }

    public bool IsGrounded
    {
        get => isGrounded;
        private set => isGrounded = value;
    }

    public bool HasJumped
    {
        get => hasJumped;
        private set => hasJumped = value;
    }

    public bool IsReadyToJump
    {
        get => isReadyToJump;
        private set => isReadyToJump = value;
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

    public void UpdateStateBasedOnInput(Vector2 inputVector)
    {
        isWalking = inputVector != Vector2.zero;
        isRunning = isRunning && inputVector != Vector2.zero;
    }

    public void HandleRunningStateChange()
    {
        isRunning = !isRunning && isWalking;
        entityRigidBody.EntitySpeed = isRunning ? runSpeed : moveSpeed;
    }

    public void HandleJumpingState()
    {
        if (isReadyToJump && isGrounded)
        {
            hasJumped = true;
            isReadyToJump = false;
            entityRigidBody.Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        isReadyToJump = true;
        hasJumped = false;
    }
}
