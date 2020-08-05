using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : LivingEntity
{
    // Components + Variables
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

    [Header("Player Misc Properties")]
    private bool controlsEnabled;

    [Header("Fire Ball Properties")]
    [SerializeField] private int fireBallDamageAmount = 10;
    [SerializeField] private int fireBallEnergyCost = 10;
    [SerializeField] private float fireBallBaseCooldown = 0;
    private float fireballCurrentCooldown;

    [Header("Blink Properties")]
    [SerializeField] private int blinkEnergyCost = 10;
    [SerializeField] private float blinkBaseCooldown = 0;
    private float blinkCurrentCooldown;

    [Header("Mana Barrier Properties")]
    [SerializeField] private int mbEnergyCostPerTick;
    [HideInInspector] public bool manaBarrierIsActive;
    [SerializeField] private GameObject manaBarrierVisualParent;
    #endregion

    // Initialization + Setup + Update
    #region
    public override void InitializeSetup()
    {
        base.InitializeSetup();

        // Fill up energy bar, start gain energy over time
        ModifyCurrentEnergy(maxEnergy);
        StartCoroutine(GainEnergyPassively());
        controlsEnabled = true;
    }
    private void Start()
    {
        InitializeSetup();
        DisableControls();
    }  
    private void Update()
    {
        if (controlsEnabled)
        {
            Move();
            ShootFireBall();
            LookAtMouse();
            PerformBlink();
            AwaitManaBarrierInput();
        }        
    }
    public void ResetToStartSettings()
    {
        myCharacterAnimator.ShowModel();
        myCharacterAnimator.PlayIdleAnimation();
        transform.position = new Vector3(0, 0, 0);
        ModifyHealth(maxHealth);
        inDeathProcess = false;
        myBoxCollider.enabled = true;
        controlsEnabled = true;
    }
    public void DisableControls()
    {
        controlsEnabled = false;
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
        newX = Mathf.Clamp(newX, WorldManager.Instance.XMin, WorldManager.Instance.XMax);
        newY = Mathf.Clamp(newY, WorldManager.Instance.YMin, WorldManager.Instance.YMax);

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
    private void PerformBlink()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 mousePos = CameraManager.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (WorldManager.Instance.IsLocationInBounds(mousePos) &&
                HasEnoughEnergy(blinkEnergyCost, currentEnergy))
            {
                transform.position = mousePos;
                ModifyCurrentEnergy(-blinkEnergyCost);
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
    public override void SetDeathProcess()
    {
        base.SetDeathProcess();
        controlsEnabled = false;
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

    // Mana Barrier Logic
    #region
    private void AwaitManaBarrierInput()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1) && 
            currentEnergy > 5 &&
            manaBarrierIsActive == false)
        {
            ActivateManaBarrier();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            CancelManaBarrier();
        }
    }
    private void ActivateManaBarrier()
    {
        manaBarrierIsActive = true;
        SetManaBarrierEffectViewState(true);
        StartCoroutine(ChannelManaBarrier());
    }
    private IEnumerator ChannelManaBarrier()
    {
        while(manaBarrierIsActive && currentEnergy > 5)
        {
            ModifyCurrentEnergy(-mbEnergyCostPerTick);
            yield return new WaitForSeconds(0.1f);
        }
        CancelManaBarrier();
    }
    private void CancelManaBarrier()
    {
        manaBarrierIsActive = false;
        SetManaBarrierEffectViewState(false);
    }
    private void SetManaBarrierEffectViewState(bool onOrOff)
    {
        manaBarrierVisualParent.SetActive(onOrOff);
    }
    #endregion
}
