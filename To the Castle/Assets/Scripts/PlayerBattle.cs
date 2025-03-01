using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;

    [SerializeField] private float attackCooldown = 2.2f;

    private bool isAttacking;

    private void Start()
    {
        gameInput.OnAttackAction += GameInput_OnAttackAction;
    }

    private void GameInput_OnAttackAction(object sender, System.EventArgs e)
    {
        if(!isAttacking)
        {
            isAttacking = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}
