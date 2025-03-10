using UnityEngine;

public class StatesAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_IN_AIR = "IsInAir";
    private const string HAS_JUMPED = "HasJumped";
    private const string IS_ATTACKING = "IsAttacking";

    [SerializeField] private IEntityState IState;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool(IS_WALKING, IState.IsWalking);
        animator.SetBool(IS_RUNNING, IState.IsRunning);     
        animator.SetBool(IS_IN_AIR, IState.IsInAir());
        animator.SetBool(HAS_JUMPED, IState.HasJumped);
        animator.SetBool(IS_ATTACKING, IState.IsAttacking);
    }
}
