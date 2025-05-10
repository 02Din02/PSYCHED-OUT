using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private float health;
    private float stamina;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float maxStamina = 100;
    private PlayerMovement playerMovement;
    [SerializeField] private Animator playerAnimator;
    private SetupScript setupScript;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private UIBar healthBar;
    [SerializeField] private Slider StaminaSlider;
    [SerializeField] private UIBar StaminaBar;
    private bool dying = false;

    [SerializeField] private DataManager dataManager;
    [SerializeField] private BossController bossController;
    private AudioManager audioM;

    void Start()
    {
        health = maxHealth;
        stamina = maxStamina;
        playerMovement = GetComponent<PlayerMovement>();
        dataManager = FindObjectOfType<DataManager>();
        setupScript = FindObjectOfType<SetupScript>();
        audioM = FindObjectOfType<AudioManager>();

        SetupHealthSlider();
    }

    void Update()
    {
        if (health <= 0)
        {
            PlayerDeath();
        }

        ////take damage button
        //if (Input.GetKeyDown(KeyCode.LeftAlt))
        //{
        //    TakeDamage(10);
        //}

        // Update maxHealth and slider if it changes so no overflow happens
        if (maxHealth != playerMovement.Stats.MaxHealth)
        {
            maxHealth = playerMovement.Stats.MaxHealth;
            SetupHealthSlider();
        }
        if (maxStamina != playerMovement.Stats.MaxStamina)
        {
            maxStamina = playerMovement.Stats.MaxStamina;
            SetupStaminaSlider();
        }
    }

    public void SetupHealthSlider()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = healthSlider.maxValue;
        healthBar.ChangeBar(healthSlider.maxValue);
    }
    public void SetupStaminaSlider()
    {
        StaminaSlider.maxValue = maxStamina;
        StaminaSlider.value = StaminaSlider.maxValue;
        StaminaBar.ChangeBar(StaminaSlider.maxValue);
    }

    private void PlayerDeath()
    {
        if (!dying)
        {
            dying = true;
            audioM.PlaySound(audioM.die);
            playerAnimator.SetBool("Death", true);
            dataManager.attemptNum += 1;
            float damageDone = bossController.maxhealth - bossController.health;
            float percentDamage = (damageDone / bossController.maxhealth) * 100f;
            dataManager.currency = Mathf.RoundToInt(percentDamage);

            playerMovement.enabled = false;
            setupScript.FadeIn();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void TakeDamage(int damage, Vector2? direction = null)
    {
        if (direction == null)
        {
            direction = Vector2.left * 14f;
        }
        audioM.PlaySound(audioM.hurt);
        health -= damage;
        if (health < 0)
        {
            health = 0;
        }
        
        playerMovement.AddFrameForce((Vector2)direction, true);
        playerMovement.ApplyHitstun(damage);

        healthSlider.value = health;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void ResetHealth()
    {
        health = maxHealth;
        SetupHealthSlider();
        SetupStaminaSlider();
    }
}
