using UnityEngine;

public class GoblinFighter : Enemy
{
    // Routines
    #region
    protected override void RunMyRoutines()
    {
        base.RunMyRoutines();

        if (inDeathProcess || currentTarget == null || currentTarget.inDeathProcess)
        {
            return;
        }

        bool inRange = CombatLogic.Instance.IsTargetInRange(attackRange, this, currentTarget);

        if (!inRange)
        {
            MoveTowardsPlayer();
        }
        else if(inRange && attackCurrentCooldown == 0)
        {
            AttackPlayer();
        }
    }
    void AttackPlayer()
    {
        myCharacterAnimator.PlayMeleeAttackAnimation();
        CombatLogic.Instance.HandleDamage(currentTarget, attackDamage);
        SetAttackOnCooldown();
    }
    #endregion

}
