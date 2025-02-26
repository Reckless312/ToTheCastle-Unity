using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerObject;
    [SerializeField] private ThirdPersonCamera thirdPersonCamera;
    [SerializeField] private LayerMask gateLayerMask;
    [SerializeField] private GameObject stepRayUpper;
    [SerializeField] private GameObject stepRayLower;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stepHeight = 0.3f;
    [SerializeField] private float stepSmooth = 0.1f;

    private Vector3 lastInteractDirection;

    private bool isWalking;
    private bool isRunning;

    private void Awake()
    {
        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnRunAction += GameInput_OnRunAction;
    }

    private void GameInput_OnRunAction(object sender, System.EventArgs e)
    {
        isRunning = !isRunning && isWalking;
        moveSpeed = isRunning ? 8f : 5f;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 directionVector = orientation.forward * inputVector.y + orientation.right * inputVector.x;

        if (directionVector != Vector3.zero)
        {
            lastInteractDirection = directionVector;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, gateLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out MainGate mainGate))
            {
                mainGate.Interact();
            }
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleStairs();
    }

    private void HandleMovement()
    {
        UpdateOrientation();

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 directionVector = orientation.forward * inputVector.y + orientation.right * inputVector.x;

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.3f;
        float playerHeight = 1.5f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionVector, moveDistance);

        if (!canMove)
        {
            Vector3 directionVectorX = new Vector3(directionVector.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionVectorX, moveDistance);
            if (canMove)
            {
                directionVector = directionVectorX;
            }
            else
            {
                Vector3 directionVectorZ = new Vector3(0, 0, directionVector.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionVectorZ, moveDistance);
                if (canMove)
                {
                    directionVector = directionVectorZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += directionVector * moveDistance;
        }

        float rotationSpeed = 10f;

        if (directionVector != Vector3.zero)
        {
            playerObject.forward = Vector3.Slerp(playerObject.forward, directionVector.normalized, Time.deltaTime * rotationSpeed);
        }

        isWalking = inputVector != Vector2.zero;
        isRunning = isRunning && inputVector != Vector2.zero;
    }

    private void HandleStairs()
    {
        float stepDistance = 0.1f;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitLower, stepDistance))
        {
            Debug.Log("Lower hit");
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitUpper, stepDistance + 0.1f))
            {
                Debug.Log("Upper did not hit");
                transform.position += new Vector3(0, +stepHeight, 0);
            }
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    private void UpdateOrientation()
    {
        Vector3 thirdPersonCameraPosition = thirdPersonCamera.transform.position;
        orientation.forward = (transform.position - new Vector3(thirdPersonCameraPosition.x, transform.position.y, thirdPersonCameraPosition.z)).normalized;
    }
}
