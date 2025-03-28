using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


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
        TwoHitCombo();
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

    void TwoHitCombo()
    {
        float first_hit_dmg = 20f;
        float second_hit_dmg = 30f;

        Vector3 hitbox_pos = boss.transform.position + new Vector3(2,0,0);
        Vector3 hitbox_size = new Vector3(2,2,2);



        Physics.OverlapBox(new Vector3(0,0,0), new Vector3(1,1,1));
        Debug.Log(hitbox_pos);
        Debug.Log(hitbox_size);

        Gizmos.DrawWireCube(hitbox_pos, hitbox_size);
        Debug.Log("GOT HERE");

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

    void OnDrawGizmos() {
        // Vector3 hitbox_size = new Vector3(5,4,0);
        // Vector3 hitbox_pos = boss.transform.position + new Vector3(-2, boss.transform.h,0);
        // Gizmos.DrawWireCube(hitbox_pos, hitbox_size);
        return;
    }
}