using UnityEngine;
using UnityEngine.AI;

public class EnemyActions : MonoBehaviour
{
    [SerializeField] private NavMeshAgent meshAgent;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float walkPointRange = 5;
    [SerializeField] private float timeBetweenAttacks = 2.0f;

    private PlayerEvents playerEvents;
    private EnemyState enemyState;

    public Vector3 walkPoint;

    private bool walkPointSet;
    private bool alreadyAttacked;

    private void Awake()
    {
        meshAgent = GetComponent<NavMeshAgent>();
        enemyState = GetComponent<EnemyState>();

        playerEvents = DoNotDestroy.PlayerEvents;
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
        meshAgent.SetDestination(playerEvents.transform.position);
        enemyState.IsWalking = true;
    }

    public void AttackPlayer()
    {
        enemyState.IsWalking = false;

        meshAgent.SetDestination(transform.position);

        transform.LookAt(playerEvents.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !alreadyAttacked)
        {
            enemyState.IsAttacking = true;
            playerEvents.HandleDamage(enemyState.Damage);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        enemyState.IsAttacking = false;
        alreadyAttacked = false;
    }
}
