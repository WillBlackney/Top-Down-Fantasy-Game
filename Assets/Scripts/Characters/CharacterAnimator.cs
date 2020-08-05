using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public Animator animator;
    public GameObject modelParent;

    [Header("Properties")]
    public bool attackAnimActive;
    public bool shootMomentReached;
    #endregion

    // Trigger animations
    #region
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
    #endregion

    // Misc Logic
    #region
    public void ShowModel()
    {
        modelParent.SetActive(true);
    }
    public void HideModel()
    {
        modelParent.SetActive(false);
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
    #endregion
}
