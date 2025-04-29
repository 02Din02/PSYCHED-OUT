using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private float health;
    [SerializeField] private float maxHealth = 100;
    private PlayerMovement playerMovement;
    private RestartScene restartLevel;
    [SerializeField] private Slider healthSlider;
    private bool dying = false;

    [SerializeField] private DataManager dataManager;
    [SerializeField] private BossController bossController;

    void Start()
    {
        health = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
        dataManager = FindObjectOfType<DataManager>();
        restartLevel = FindObjectOfType<RestartScene>();

        SetupHealthSlider();
    }

    void Update()
    {
        if (health <= 0)
        {
            PlayerDeath();
        }

        //take damage button
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            TakeDamage(10);
        }

        // Update maxHealth and slider if it changes so no overflow happens
        if (maxHealth != playerMovement.Stats.MaxHealth)
        {
            maxHealth = playerMovement.Stats.MaxHealth;
            SetupHealthSlider();
        }
    }

    private void SetupHealthSlider()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    private void PlayerDeath()
    {
        if (!dying)
        {
            dying = true;
            float damageDone = bossController.maxhealth - bossController.health;
            float percentDamage = (damageDone / bossController.maxhealth) * 100f;
            dataManager.currency += Mathf.RoundToInt(percentDamage);

            playerMovement.enabled = false;
            restartLevel.FadeIn();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void TakeDamage(int damage, Vector2? direction = null)
    {
        if (direction == null)
        {
            direction = Vector2.left * 14f;
        }

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
    }
}
