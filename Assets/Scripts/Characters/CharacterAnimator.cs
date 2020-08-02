using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [Header("Component References")]
    public Animator animator;

    [Header("Properties")]
    public bool attackAnimActive;
    public bool shootMomentReached;

    public void PlayMeleeAttackAnimation()
    {
        SetAttackAnimState();
        animator.SetTrigger("Melee Attack");
    }
    public void PlayRangedAttackAnimation()
    {
        SetAttackAnimState();
        animator.SetTrigger("Ranged Attack");
    }
    public void PlayWalkAnimation()
    {
        if (!attackAnimActive)
        {
            animator.SetTrigger("Walk");
        }
    }
    public void PlayIdleAnimation()
    {
        if (!attackAnimActive)
        {
            animator.SetTrigger("Idle");
        }
    }
    public void PlayDeathAnimation()
    {
        DisableAttackAnimState();
        animator.SetTrigger("Die");
    }

    public void SetAttackAnimState()
    {
        attackAnimActive = true;
    }
    public void DisableAttackAnimState()
    {
        attackAnimActive = false;
    }
    public void ShootMomentReached()
    {
        shootMomentReached = true;
    }
}
