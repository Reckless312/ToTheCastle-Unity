using UnityEngine;
using UnityEngine.AI;

public class EnemyState : MonoBehaviour, IEntityState
{
    private bool isWalking;
    private bool isRunning;
    private bool isJumping;
    private bool isGrounded;
    private bool isAttacking;
    private bool hasJumped;

    private void Awake()
    {
        isRunning = false;
        isJumping = false;
        isGrounded = true;
        hasJumped = false;
    }

    public bool IsWalking
    {
        get => isWalking;
        set => isWalking = value;
    }

    public bool IsRunning{
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

    public bool IsAttacking
    {
        get => isAttacking;
        set => isAttacking = value;
    }

    public bool HasJumped
    {
        get => hasJumped;
        private set => hasJumped = value;
    }

    public bool IsInAir()
    {
        return false;
    }
}
