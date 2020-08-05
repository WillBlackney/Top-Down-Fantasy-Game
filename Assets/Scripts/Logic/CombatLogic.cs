using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatLogic : Singleton<CombatLogic>
{
    public void HandleDamage(LivingEntity entityDamaged, int damageValue)
    {
        Player player = entityDamaged.GetComponent<Player>();
        if(player && player.manaBarrierIsActive == true)
        {
            // do mana barrier reflection VFX stuff here
        }
        else
        {
            entityDamaged.ModifyHealth(-damageValue);
            if (entityDamaged.currentHealth <= 0)
            {
                HandleDeath(entityDamaged);
            }
        }   

    }
    public void HandleDeath(LivingEntity entityKilled)
    {
        StartCoroutine(HandleDeathCoroutine(entityKilled));
    }
    private IEnumerator HandleDeathCoroutine(LivingEntity entityKilled)
    {
        // To do: play death anim, trigger death rattle effects, etc
        if(entityKilled is Enemy)
        {            
            // Get enemy script
            Enemy enemy = entityKilled.GetComponent<Enemy>();

            // Set death state
            enemy.SetDeathProcess();

            // Gain score on kill
            ScoreManager.Instance.ModifyScore(enemy.scoreGainOnKill);

            // Play death anim
            enemy.myCharacterAnimator.PlayDeathAnimation();
            yield return new WaitForSeconds(1);

            // Remove from data persistency, destroy game object
            EnemySpawnManager.Instance.RemoveEnemyFromAllEnemiesList(enemy);
            Destroy(entityKilled.gameObject);

            // check if last enemy in wave
            if (EnemySpawnManager.Instance.allEnemies.Count == 0 &&
                EnemySpawnManager.Instance.activelySpawning == false)
            {
                // do 'wave completed' stuff
                EventManager.Instance.StartNewWaveCompletedEvent();
            }
        }      
        else if (entityKilled is Player)
        {
            // Stop spawning enemies
            EnemySpawnManager.Instance.CancelSpawning();

            // Get enemy script
            Player player = entityKilled.GetComponent<Player>();

            // Set death state
            player.SetDeathProcess();            

            // Play death anim
            player.myCharacterAnimator.PlayDeathAnimation();
            yield return new WaitForSeconds(1);

            // Hide player GO
            player.myCharacterAnimator.HideModel();

            // Trigger end game defeat event
            EventManager.Instance.StartNewGameOverDefeatEvent();
        }

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
