using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingEntity
{
    // Properties + Components
    #region
    [Header("Enemy Component References")]
    public GameObject flipParent;
    public SpriteRenderer mySpriteRenderer;

    [Header("Enemy Properties")]
    public Player currentTarget;
    public int scoreGainOnKill;
    public int attackDamage;
    public int attackRange;
    public float attackBaseCooldown;
    public float attackCurrentCooldown;
    #endregion

    // Initialization + Setup + Update
    #region
    public override void InitializeSetup()
    {
        base.InitializeSetup();
        AutoFindAndSetPlayerTarget();
    }    
    protected void Update()
    {
        RunMyRoutines();
    }
    #endregion

    // Core Logic
    #region
    protected virtual void RunMyRoutines()
    {
        ReduceCooldownInUpdate();
        FaceDirectionOfTravel(transform.position.x, currentTarget.transform.position.x);
    }
    public void AutoFindAndSetPlayerTarget()
    {
        currentTarget = FindObjectOfType<Player>();
    }
    protected virtual void MoveTowardsPlayer()
    {
        if(transform.position != currentTarget.transform.position)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, currentTarget.transform.position, Time.deltaTime * moveSpeed);
            transform.position = newPos;
        }
    }
    protected virtual void SetAttackOnCooldown()
    {
        attackCurrentCooldown = attackBaseCooldown;
    }
    private void ReduceCooldownInUpdate()
    {
        if(attackCurrentCooldown > 0)
        {
            attackCurrentCooldown -= Time.deltaTime;

            // prevent dropping cooldown below 0
            if(attackCurrentCooldown < 0)
            {
                attackCurrentCooldown = 0;
            }
        }
    }
    #endregion


}
