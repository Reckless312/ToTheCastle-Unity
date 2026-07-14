using UnityEngine;

public interface IEntityState
{
    public bool IsWalking { get; }

    public bool IsRunning { get; }

    public bool IsJumping { get; }

    public bool IsGrounded { get; }

    public bool IsAttacking { get; }

    public bool IsAlive { get; }

    public bool HasJumped { get; }

    public bool IsInAir();
}
