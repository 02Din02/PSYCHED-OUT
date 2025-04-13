using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private int health;
    [SerializeField] private int maxHealth = 100;
    private PlayerMovement playerMovement;
    [SerializeField] SceneTransitionScript sceneTransition;
    [SerializeField] HealthBarScript healthBarScript;

    [SerializeField] DataManager dataManager;
    [SerializeField] BossController bossController;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
        dataManager = FindObjectOfType<DataManager>();
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
        StartCoroutine(PlayerDeathSequence());
        GetComponent<BoxCollider2D>().enabled = false;
        //sceneTransition.FadeOut();
    }
    private IEnumerator PlayerDeathSequence()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        sceneTransition.FadeOut();

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0;
            return;
        }
        float healthpercent = (float)damage / maxHealth;
        healthBarScript.ChangeHealth(-healthpercent);
        Debug.Log(health);
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
