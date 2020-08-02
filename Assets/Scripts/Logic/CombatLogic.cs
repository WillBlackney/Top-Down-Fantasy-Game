using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatLogic : Singleton<CombatLogic>
{
    public void HandleDamage(LivingEntity entityDamaged, int damageValue)
    {
        entityDamaged.ModifyHealth(-damageValue);
        if(entityDamaged.currentHealth <= 0)
        {
            HandleDeath(entityDamaged);
        }

    }
    public void HandleDeath(LivingEntity entityKilled)
    {
        // To do: play death anim, trigger death rattle effects, etc
        if(entityKilled is Enemy)
        {
            // Gete enemy script
            Enemy enemy = entityKilled.GetComponent<Enemy>();

            // Gain score on kill
            ScoreManager.Instance.ModifyScore(enemy.scoreGainOnKill);
            // check if last enemy in wave
        }
        Destroy(entityKilled.gameObject);
    }
    public float GetDistanceBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b);
    }
    public bool IsTargetInRange(int range, LivingEntity attacker,LivingEntity target)
    {
        return GetDistanceBetweenTwoPoints(attacker.transform.position, target.transform.position) <= range;
    }
    

    
}
