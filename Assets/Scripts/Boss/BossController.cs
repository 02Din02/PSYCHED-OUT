using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    //gameobjects
    public GameObject boss;
    public PlayerMovement player;

    // Values
    public float bossHealth;

    // UI
    public Slider healthSlider;

    //ranges (____[L]____lrange____[M]____mrange____[C]____)
    public int lrange;
    public int mrange;

    //movesets for each range
    public List<string> lset;
    public List<string> mset;
    public List<string> cset;

    public bool attacking;
    public float dist;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        // Find the player
        healthSlider.GetComponent<Slider>();
        // Get the Slider script of the boss health bar

        healthSlider.maxValue = bossHealth;
        healthSlider.value = bossHealth;
        // set the health on the boss health bar
    }
    void attack()
    {
        attacking = true;
        string a;
        if (dist >= lrange) {
            a = lset[Random.Range(0, lset.Count)];
        }
        else if (dist >= mrange) {
            a = mset[Random.Range(0, mset.Count)];
        }
        else
        {
            a = cset[Random.Range(0, cset.Count)];
        }

        Invoke(a, 0);
    }

    void reposition(int x)
    {
        if (x == 2)
        {
            //reposition so player is past lrange
        }
        else if (x == 1)
        {
            //reposition so player is between lrange and mrange
        }
        else if (x == 0)
        {
            //reposition so player is before mrange
        }
        else
        {
            return;
        }
        attack();
    }

    void lattack1()
    {
        return;
    }

    void mattack1()
    {
        return;
    }

    void cattack1()
    {
        return;
    }

    void cattack2()
    {
        return;
    }

    void UpdateHealthBar()
    {
        // Should get called every time Player hits boss, NOT IN UPDATE!!!!!!
        healthSlider.value = bossHealth;
    }

    void Update()
    {
        dist = player.transform.position.x - boss.transform.position.x;

        if (!attacking)
        {
            attack();
        }
    }
}