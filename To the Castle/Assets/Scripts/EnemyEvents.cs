using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsPlayer;

    [SerializeField] private float sightRange = 20;
    [SerializeField] private float attackRange = 0.8f;

    private EnemyActions enemyActions;

    private bool playerInSightRange;
    private bool playerInAttackRange;

    private void Awake()
    {
        enemyActions = GetComponent<EnemyActions>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) enemyActions.Patrolling();
        if (playerInSightRange && !playerInAttackRange) enemyActions.ChasePlayer();
        if (playerInSightRange && playerInAttackRange) enemyActions.AttackPlayer();
    }

    public void PlayerAttacked(float damage)
    {
        enemyActions.TakeDamage(damage);
    }
}
