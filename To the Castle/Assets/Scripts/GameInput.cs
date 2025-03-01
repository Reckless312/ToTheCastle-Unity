using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnRunAction;
    public event EventHandler OnJumpAction;
    public event EventHandler OnAttackAction;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += InteractPerformed;

        playerInputActions.Player.Run.performed += RunPerformed;
        playerInputActions.Player.Run.canceled += RunPerformed;

        playerInputActions.Player.Jump.performed += JumpPerformed;

        playerInputActions.Player.Attack.performed += AttackPerformed;
    }

    private void AttackPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAttackAction?.Invoke(this, EventArgs.Empty);
    }

    private void JumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJumpAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void RunPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnRunAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
