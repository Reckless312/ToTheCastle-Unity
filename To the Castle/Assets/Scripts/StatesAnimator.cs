using UnityEngine;

public class StatesAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_IN_AIR = "IsInAir";
    private const string HAS_JUMPED = "HasJumped";
    private const string IS_ATTACKING = "IsAttacking";
    private const string IS_ALIVE = "IsAlive";

    [SerializeField] private MonoBehaviour component;

    private Animator animator;
    private IEntityState IState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        IState = component as IEntityState;
        if(IState == null)
        {
            Debug.LogError("Component does not implement IEntityState");
        }
    }

    void Update()
    {
        animator.SetBool(IS_WALKING, IState.IsWalking);
        animator.SetBool(IS_RUNNING, IState.IsRunning);     
        animator.SetBool(IS_IN_AIR, IState.IsInAir());
        animator.SetBool(HAS_JUMPED, IState.HasJumped);
        animator.SetBool(IS_ATTACKING, IState.IsAttacking);
        animator.SetBool(IS_ALIVE, IState.IsAlive);
    }
}
