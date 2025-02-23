using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerObject;
    [SerializeField] private ThirdPersonCamera thirdPersonCamera;

    [SerializeField] private float moveSpeed = 5f;

    private bool isWalking;

    private void Update()
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
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void UpdateOrientation()
    {
        Vector3 thirdPersonCameraPosition = thirdPersonCamera.transform.position;
        orientation.forward = (transform.position - new Vector3(thirdPersonCameraPosition.x, transform.position.y, thirdPersonCameraPosition.z)).normalized;
    }
}
