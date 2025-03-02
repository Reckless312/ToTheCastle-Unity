using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("Game Objects References")]

    [SerializeField] private Transform playerObject;

    [Header("Hierarchy References")]

    private EntityRigidBody entityRigidBody;
    private PlayerEvents playerEvents;

    [Header("Player Settings")]

    [SerializeField] private float playerHeight = 1.75f;

    private void Awake()
    {
        playerEvents = GetComponent<PlayerEvents>();

        entityRigidBody = GetComponent<EntityRigidBody>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 directionVector = playerEvents.GetDirectionVector();

        float moveDistance = entityRigidBody.EntitySpeed * Time.deltaTime;
        float playerRadius = 0.3f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionVector, moveDistance);

        if (!canMove)
        {
            Vector3 directionVectorX = new Vector3(directionVector.x, 0, 0);
            canMove = directionVector.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionVectorX, moveDistance);
            if (canMove)
            {
                directionVector = directionVectorX;
            }
            else
            {
                Vector3 directionVectorZ = new Vector3(0, 0, directionVector.z);
                canMove = directionVector.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, directionVectorZ, moveDistance);
                if (canMove)
                {
                    directionVector = directionVectorZ;
                }
            }
        }

        if (canMove)
        {
            entityRigidBody.Push(directionVector);
        }

        float rotationSpeed = 10f;

        if (directionVector != Vector3.zero)
        {
            playerObject.forward = Vector3.Slerp(playerObject.forward, directionVector.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
