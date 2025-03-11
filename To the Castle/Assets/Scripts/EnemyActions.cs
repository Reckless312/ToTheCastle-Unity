using UnityEngine;
using UnityEngine.AI;

public class EnemyActions : MonoBehaviour
{
    [SerializeField] private NavMeshAgent meshAgent;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float walkPointRange = 5;
    [SerializeField] private float timeBetweenAttacks = 0.5f;

    private Transform player;
    private EnemyState enemyState;

    public Vector3 walkPoint;

    private bool walkPointSet;
    private bool alreadyAttacked;

    private void Awake()
    {
        meshAgent = GetComponent<NavMeshAgent>();
        enemyState = GetComponent<EnemyState>();

        player = DoNotDestroy.PlayerEvents.transform;
    }

    public void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    public void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            meshAgent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
        else
            enemyState.IsWalking = true;
    }

    public void ChasePlayer()
    {
        meshAgent.SetDestination(player.position);
        enemyState.IsWalking = true;
    }

    public void AttackPlayer()
    {
        enemyState.IsWalking = false;

        meshAgent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
