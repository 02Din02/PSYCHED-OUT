using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private int health;
    [SerializeField] private int maxHealth = 100;
    private PlayerMovement playerMovement;
    [SerializeField] SceneTransitionScript sceneTransition;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            PlayerDeath();
        }
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
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
        if(health < 0)
        {
            health = 0;
            return;
        }
        health -= damage;
    }

    public int GetHealth()
    {
        return health;
    }
    
}
