using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private int health;
    [SerializeField] private int maxHealth = 100;
    private PlayerMovement playerMovement;
    private RestartScene restartLevel;
    [SerializeField] private Slider healthSlider;

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
    }

    private void PlayerDeath()
    {
        dataManager.currency += (int)(bossController.health * 100 / bossController.maxhealth);
        playerMovement.enabled = false;
        restartLevel.FadeIn();
        GetComponent<BoxCollider2D>().enabled = false;
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

    public int GetHealth()
    {
        return health;
    }
    
    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
