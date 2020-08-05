using UnityEngine;

public class Enemy : LivingEntity
{
    // Properties + Components
    #region
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
        // Main function for dictating enemy behaviour
        // Should be overriden in every enemy sub script, but
        // also include the base RunMyRoutines logic
        if (inDeathProcess || currentTarget == null || currentTarget.inDeathProcess)
        {
            return;
        }

        ReduceCooldownInUpdate();
        FaceDirectionOfTravel(transform.position.x, currentTarget.transform.position.x);
    }
    public void AutoFindAndSetPlayerTarget()
    {
        // TO DO in future: player should be stored in a static variable, (unless we want mutliple players?)
        // find object of type will degrade performanc if we spawn too many enemies
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
    protected virtual void MoveTowardsWorldCentre()
    {
        // used to make enemies move in bounds BEFORE
        // starting their normal routines.
        // this prevents enemies from attacking from the woods and hidden areas
        if (transform.position != WorldManager.Instance.GetWorldCentre())
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, WorldManager.Instance.GetWorldCentre(), Time.deltaTime * moveSpeed);
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
