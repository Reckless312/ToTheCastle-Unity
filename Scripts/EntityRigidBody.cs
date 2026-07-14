using UnityEngine;

public class EntityRigidBody : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float groundDrag = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float airMultiplier = 0.5f;

    private Rigidbody rigitBody;

    private float entitySpeed;

    public float EntitySpeed
    {
        get => entitySpeed;
        set => entitySpeed = value;
    }

    private bool isGrounded;

    private void Awake()
    {
        rigitBody = GetComponent<Rigidbody>();
        rigitBody.freezeRotation = true;
    }

    private void Update()
    {
        IsGrounded();
        HandleDrag();
        SpeedControl();
    }

    public bool IsGrounded()
    {
        float raycastDistance = 0.2f;
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, raycastDistance, groundMask);
        return isGrounded;
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rigitBody.linearVelocity.x, 0, rigitBody.linearVelocity.z);
        if (flatVelocity.magnitude > entitySpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * entitySpeed;
            rigitBody.linearVelocity = new Vector3(limitedVelocity.x, rigitBody.linearVelocity.y, limitedVelocity.z);
        }
    }

    public void Push(Vector3 directionVector)
    {
        float speedMultiplier = 10f;
        if (isGrounded)
        {
            rigitBody.AddForce(directionVector * entitySpeed * speedMultiplier, ForceMode.Force);
        }
        else
        {
            rigitBody.AddForce(directionVector * entitySpeed * speedMultiplier * airMultiplier, ForceMode.Force);
        }
    }

    public void Jump()
    {
        rigitBody.angularVelocity = new Vector3(rigitBody.angularVelocity.x, 0, rigitBody.angularVelocity.z);
        rigitBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void HandleDrag()
    {
        if (isGrounded)
        {
            rigitBody.linearDamping = groundDrag;
        }
        else
        {
            rigitBody.linearDamping = 0;
        }
    }
}
