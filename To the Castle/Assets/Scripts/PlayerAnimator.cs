using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_IN_AIR = "IsInAir";
    private const string HAS_JUMPED = "HasJumped";
    private const string IS_ATTACKING = "IsAttacking";

    [SerializeField] private PlayerState playerState;
    [SerializeField] private PlayerBattle playerBattle;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool(IS_WALKING, playerState.IsWalking);
        animator.SetBool(IS_RUNNING, playerState.IsRunning);     
        animator.SetBool(IS_IN_AIR, playerState.IsInAir());
        animator.SetBool(HAS_JUMPED, playerState.HasJumped);
        animator.SetBool(IS_ATTACKING, playerBattle.IsAttacking());
    }
}
