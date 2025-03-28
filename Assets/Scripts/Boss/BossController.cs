using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;

public class BossController : MonoBehaviour
{
    //gameobjects
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

    int facing = -1; //1 for right, -1 for left

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        // Find the player
        healthSlider.GetComponent<Slider>();
        // Get the Slider script of the boss health bar

        healthSlider.maxValue = bossHealth;
        healthSlider.value = bossHealth;
        // set the health on the boss health bar
        laser_orb();
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

    public GameObject laser_orb_prefab;
    float h_dist = 7F;
    float v_dist = 3F;
    void laser_orb()
    {
        Vector3 pos_1 = new Vector3(h_dist * facing, v_dist, 0) + transform.position;
        Vector3 pos_2 = new Vector3(h_dist * facing, 0, 0) + transform.position;
        Vector3 pos_3 = new Vector3(h_dist * facing, -v_dist, 0) + transform.position;

        Debug.Log("Orbs created");

        GameObject orb1 = Instantiate(laser_orb_prefab, pos_1, Quaternion.identity);
        GameObject orb2 = Instantiate(laser_orb_prefab, pos_2, Quaternion.identity);
        GameObject orb3 = Instantiate(laser_orb_prefab, pos_3, Quaternion.identity);

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
        dist = player.transform.position.x - transform.position.x;
        if (dist > 0)
        {
            facing = -1;
        }
        else
        {
            facing = 1;
        }

        if (!attacking)
        {
            attack();
        }
    }
}