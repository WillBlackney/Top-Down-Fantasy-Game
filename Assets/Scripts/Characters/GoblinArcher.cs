using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinArcher : Enemy
{
    private bool alreadyShooting;
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
        else if (inRange && attackCurrentCooldown == 0 && !alreadyShooting)
        {
            ShootPlayer();
        }
    }    
    void ShootPlayer()
    {
        alreadyShooting = true;
        StartCoroutine(ShootPlayerCoroutine());
    }

    IEnumerator ShootPlayerCoroutine()
    {
        // Trigger attack anim
        myCharacterAnimator.PlayRangedAttackAnimation();

        // Wait for animation to hit the sweet spot, then refresh
        yield return new WaitUntil(()=> myCharacterAnimator.shootMomentReached == true);
        myCharacterAnimator.shootMomentReached = false;

        // Instantiate prefab, get and set up projectile script
        Projectile arrow = Instantiate(PrefabHolder.Instance.arrowPrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
        arrow.InitializeSetup(this, transform.position, currentTarget.transform.position, attackDamage);
        SetAttackOnCooldown();
        alreadyShooting = false;
    }
}
