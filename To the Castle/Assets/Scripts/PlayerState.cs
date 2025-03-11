using UnityEngine;
using UnityEngine.Playables;

public class PlayerState : MonoBehaviour, IEntityState
{
    [Header("Hierarchy References")]

    [SerializeField] private Animator playerCurrentAnimator;

    private EntityRigidBody entityRigidBody;

    [Header("Player Settings")]

    [SerializeField] private RuntimeAnimatorController exploringAnimatorController;
    [SerializeField] private RuntimeAnimatorController combatAnimatorController;
    [SerializeField] private HealthBar healthBar;

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpCooldown = 1.2f;
    [SerializeField] private float attackCooldown = 2.2f;

    [Header("Player State")]

    private bool isWalking;
    private bool isRunning;
    private bool isJumping;
    private bool isGrounded;
    private bool hasJumped;
    private bool isInAir;
    private bool isReadyToJump;
    private bool isAttacking;

    private float currentHealth;

    private void Awake()
    {
        entityRigidBody = GetComponent<EntityRigidBody>();
        entityRigidBody.EntitySpeed = moveSpeed;

        isReadyToJump = true;
        currentHealth = maxHealth;
    }

    private void Start()
    {
        healthBar.SetMaxValue(maxHealth);
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

    public bool IsAttacking
    {
        get => isAttacking;
        private set => isAttacking = value;
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

    public void HandleAttacking()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void ResetJump()
    {
        isReadyToJump = true;
        hasJumped = false;
    }

    public void ChangeAnimatorController(int indexScene)
    {
        playerCurrentAnimator.runtimeAnimatorController = indexScene == 0 ? exploringAnimatorController : combatAnimatorController;
    }

    public void WasHit(float damage)
    {
        currentHealth -= damage;
        healthBar.SetSliderValue(currentHealth);
    }
}
