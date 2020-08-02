using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour
{
    [Header("Living Entity Components")]
    public CharacterAnimator myCharacterAnimator;
    public GameObject modelParent;
    public Slider healthBar;

    [Header("Living Entity Properties")]
    public float moveSpeed;
    public int maxHealth;
    public int currentHealth;

    public virtual void InitializeSetup()
    {
        ModifyHealth(maxHealth);
    }    
    public virtual void ModifyHealth(int healthGainedOrLost)
    {
        currentHealth += healthGainedOrLost;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if(currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateMyHealthGUI();

    }

    // Animations + Model
    #region  
    protected void FaceDirectionOfTravel(float previousX, float newX)
    {
        if(newX > previousX)
        {
            FlipModelRight();
        }
        else if(newX < previousX)
        {
            FlipModelLeft();
        }
    }
    private void FlipModelRight()
    {
        modelParent.transform.localScale = new Vector3(1, 1, 1);
    }
    private void FlipModelLeft()
    {
        modelParent.transform.localScale = new Vector3(-1, 1, 1);
    }
    #endregion

    // GUI + View Logic
    #region
    protected virtual void UpdateMyHealthGUI()
    {
        UpdateHealthBarSliderPosition();
    }
    protected virtual void UpdateHealthBarSliderPosition()
    {
        float currentHealthFloat = currentHealth;
        float currentMaxHealthFloat = maxHealth;

        healthBar.value = currentHealthFloat / currentMaxHealthFloat;
    }
    #endregion
}
