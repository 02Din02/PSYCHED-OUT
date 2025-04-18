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

    [SerializeField] DataManager dataManager;
    [SerializeField] BossController bossController;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
        dataManager = FindObjectOfType<DataManager>();
        restartLevel = FindObjectOfType<RestartScene>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            PlayerDeath();
        }

        // debug keybinds
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            TakeDamage(10);
        }

        maxHealth = playerMovement.Stats.MaxHealth;
    }

    private void PlayerDeath()
    {
        if (!dying)
        {
            dying = true;
            float damageDone = bossController.maxhealth - bossController.health;
            float percentDamage = (damageDone / bossController.maxhealth) * 100f;
            dataManager.currency += Mathf.RoundToInt(percentDamage);
            // This took me like a fucking hour bro fuck this shit what the fuck is Mathf unity documentation blows
            playerMovement.enabled = false;
            restartLevel.FadeIn();
            GetComponent<BoxCollider2D>().enabled = false;
        }
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0;
            return;
        }
        playerMovement._rb.AddForce(new Vector3(-10f, 5f, 0f), ForceMode2D.Impulse);
        float healthpercent = (float)damage / maxHealth;
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

    public void resetHealth(){
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
    }
}
