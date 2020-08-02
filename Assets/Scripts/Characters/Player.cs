using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : LivingEntity
{
    // Components + Properties
    #region
    [Header("Player GUI Component References")]
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI currentEnergyText;
    [SerializeField] private TextMeshProUGUI maxEnergyText;
    [SerializeField] private Slider energyBar;

    [Header("Player Energy Properties")]
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRecovedPerTick;
    [SerializeField] private float energyRecoveryTickFrequency;
    private int currentEnergy;
    private bool passivelyGainEnergy;

    [Header("Player Move Boundary Properties")]
    [SerializeField] private float padding;
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    [Header("Fire Ball Properties")]
    [SerializeField] private int fireBallDamageAmount = 10;
    [SerializeField] private int fireBallEnergyCost = 10;
    [SerializeField] private float fireBallBaseCooldown = 0;
    private float fireballCurrentCooldown; 

    #endregion

    // Initialization + Setup + Update
    #region
    public override void InitializeSetup()
    {
        base.InitializeSetup();
        SetUpMoveBoundaries();

        // Fill up energy bar, start gain energy over time
        ModifyCurrentEnergy(maxEnergy);
        StartCoroutine(GainEnergyPassively());
    }
    private void Start()
    {
        InitializeSetup();
    }
    private void SetUpMoveBoundaries()
    {
        Camera mainCamera = Camera.main;

        xMin = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
    private void Update()
    {
        Move();
        ShootFireBall();
        LookAtMouse();
    }
    #endregion

    // Core Logic
    #region
    private void Move()
    {
        Vector3 newPos = transform.position;
        float newX = transform.position.x;
        float newY = transform.position.y;

        if (Input.GetKey(KeyCode.A))
        {
            newX -= moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            newX += moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            newY += moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            newY -= moveSpeed * Time.deltaTime;
        }

        // Clamp position to keep player on screen
        newX = Mathf.Clamp(newX, xMin, xMax);
        newY = Mathf.Clamp(newY, yMin, yMax);

        newPos = new Vector2(newX, newY);

        // did we actually move?
        if (transform.position != newPos)
        {
            // we did, play move animation
            myCharacterAnimator.PlayWalkAnimation();
        }
        else
        {
            // we did not, play idle animation
            myCharacterAnimator.PlayIdleAnimation();
        }

        // Update position 
        transform.position = new Vector2(newX, newY);   
    }
    private void ShootFireBall()
    {     
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(IsAbilityOffCooldown(fireballCurrentCooldown) && HasEnoughEnergy(fireBallEnergyCost, currentEnergy))
            {
                // Trigger attack anim
                myCharacterAnimator.PlayMeleeAttackAnimation();

                // Instantiate prefab, get and set up projectile script
                Projectile fireBall = Instantiate(PrefabHolder.Instance.fireBallPrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
                fireBall.InitializeSetup(this, transform.position, CameraManager.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition), fireBallDamageAmount);

                // Pay energy cost of ability
                ModifyCurrentEnergy(-fireBallEnergyCost);
            }            
        }
    }
    private void LookAtMouse()
    {
        float mouseX = CameraManager.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
        FaceDirectionOfTravel(transform.position.x, mouseX);
    }
    private void ModifyCurrentEnergy(int energyGainedOrLost)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + energyGainedOrLost, 0, maxEnergy);
        UpdateMyEnergyGUI();

    }
    private IEnumerator GainEnergyPassively()
    {
        passivelyGainEnergy = true;
        while (passivelyGainEnergy)
        {
            ModifyCurrentEnergy(energyRecovedPerTick);
            yield return new WaitForSeconds(energyRecoveryTickFrequency);
        }
    }

    #endregion

    // GUI Logic
    #region
    protected override void UpdateMyHealthGUI()
    {
        base.UpdateMyHealthGUI();
        UpdateCurrentHealthText();
    }
    private void UpdateCurrentHealthText()
    {
        currentHealthText.text = currentHealth.ToString();
    }   
    private void UpdateMyEnergyGUI()
    {
        UpdateEnergyBarSliderPosition();
        UpdateEnergyTexts();
    }
    private void UpdateEnergyBarSliderPosition()
    {
        float currentEnergyFloat = currentEnergy;
        float maxEnergyFloat = maxEnergy;

        energyBar.value = currentEnergyFloat / maxEnergyFloat;
    }
    private void UpdateEnergyTexts()
    {
        currentEnergyText.text = currentEnergy.ToString();
        maxEnergyText.text = maxEnergy.ToString();
    }
    #endregion

    // Conditional Bools + checkers
    #region
    private bool HasEnoughEnergy(int energyCost, int currentEnergy)
    {
        return energyCost <= currentEnergy;
    }
    private bool IsAbilityOffCooldown(float currentCooldownTime)
    {
        return currentCooldownTime == 0;
    }
    #endregion
}
