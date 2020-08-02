using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinFighter : Enemy
{
    protected override void RunMyRoutines()
    {
        base.RunMyRoutines();

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
    

}
