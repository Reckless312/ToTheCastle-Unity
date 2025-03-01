using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private const string IS_WALKING = "IsWalking";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool(IS_WALKING, enemy.IsWalking());
    }
}
