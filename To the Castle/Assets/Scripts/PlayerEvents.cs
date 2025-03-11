using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerEvents : MonoBehaviour
{
    [Header("Game Objects References")]

    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform thirdPersonCamera;
    [SerializeField] private LayerMask gateLayerMask;
    [SerializeField] private GameOver gameOver;

    [Header("Hierarchy References")]

    private PlayerState playerState;

    private Vector3 lastInteractDirection;

    private void Awake()
    {
        playerState = GetComponent<PlayerState>();
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnRunAction += GameInput_OnRunAction;
        gameInput.OnJumpAction += GameInput_OnJumpAction;
        gameInput.OnAttackAction += GameInput_OnAttackAction;
    }

    private void Update()
    {
        UpdateLastDirection();
        UpdateOrientationBasedOnCamera();
        playerState.UpdateStateBasedOnInput(gameInput.GetMovementVectorNormalized());
    }

    public void HandleChangeAnimatorController(Loader.Scene newScene)
    {
        playerState.ChangeAnimatorController(newScene);
    }

    public void HandleDamage(float damage)
    {
        playerState.CurrentHealth -= damage;
        playerState.UpdateHealthBar();

        if(playerState.CurrentHealth <= 0)
        {
            playerState.IsAlive = false;
            GetComponent<PlayerMovement>().enabled = false;
            gameInput.enabled = false;
            gameOver.ShowGameOver();
        }
    }

    private void GameInput_OnRunAction(object sender, System.EventArgs e)
    {
        playerState.HandleRunningStateChange();
    }

    private void GameInput_OnJumpAction(object sender, System.EventArgs e)
    {
        playerState.HandleJumpingState();
    }

    private void GameInput_OnAttackAction(object sender, System.EventArgs e)
    {
        float attackDistance = 1f;
        float sphereRadius = 0.5f;
        bool hitEnemy = false;

        if (Physics.SphereCast(transform.position, sphereRadius, lastInteractDirection, out RaycastHit raycastHit, attackDistance))
        {
            if (raycastHit.collider.CompareTag("Enemy"))
            {
                hitEnemy = true;
            }
        }
        playerState.HandleAttacking(hitEnemy);
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    { 
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, gateLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out MainGate mainGate))
            {
                mainGate.Interact();
            }
        }
    }

    public bool IsPlayerAlive()
    {
        return playerState.IsAlive;
    }

    private void UpdateOrientationBasedOnCamera()
    {
        orientation.forward = (transform.position - new Vector3(thirdPersonCamera.position.x, transform.position.y, thirdPersonCamera.position.z)).normalized;
    }

    public Vector3 GetDirectionVector()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 directionVector = orientation.forward * inputVector.y + orientation.right * inputVector.x;
        return directionVector;
    }

    public void UpdateLastDirection()
    {
        Vector3 directionVector = GetDirectionVector();

        if (directionVector != Vector3.zero)
        {
            lastInteractDirection = directionVector;
        }
    }
}
