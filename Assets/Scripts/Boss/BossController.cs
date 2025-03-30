using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Rendering.Universal;

public class BossController : MonoBehaviour
{
    //gameobjects
    private PlayerMovement player;
    private BoxCollider2D boxCollider;
    private GameObject laser_orb_prefab;
    private GameObject optic_pillar_prefab;
    private GameObject melee_attack_prefab;

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
        // Find the player
        player = FindObjectOfType<PlayerMovement>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Get the Slider script of the boss health bar
        healthSlider.GetComponent<Slider>();

        // set the health on the boss health bar
        healthSlider.maxValue = bossHealth;
        healthSlider.value = bossHealth;

        // load prefabs
        laser_orb_prefab = Resources.Load("Laser_Orb") as GameObject;
        optic_pillar_prefab = Resources.Load("Optic_Pillar") as GameObject;
        melee_attack_prefab = Resources.Load("Melee_Attack") as GameObject;

        // laser_orb();
        StartCoroutine(TwoHit());

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

    IEnumerator laser_orb()
    {
        float orb_dist = 2F; //distance from boss
        float orb_spread = 2F; //distance between each orb
        float orb_height = 1F; //the height of the middle orb
        float laser_spawn_delay = 0.3F; //delay between each orb spawn

        Vector3 pos_1 = new Vector3(orb_dist * facing, orb_height + orb_spread, 0) + transform.position;
        Vector3 pos_2 = new Vector3(orb_dist * facing, orb_height, 0) + transform.position;
        Vector3 pos_3 = new Vector3(orb_dist * facing, orb_height - orb_spread, 0) + transform.position;

        Debug.Log("Orbs created");

        yield return new WaitForSeconds(laser_spawn_delay);
        Instantiate(laser_orb_prefab, pos_1, Quaternion.identity);

        yield return new WaitForSeconds(laser_spawn_delay);
        Instantiate(laser_orb_prefab, pos_2, Quaternion.identity);

        yield return new WaitForSeconds(laser_spawn_delay);
        Instantiate(laser_orb_prefab, pos_3, Quaternion.identity);
    }

    void optic_pillar()
    {
        Instantiate(optic_pillar_prefab, transform.position, Quaternion.identity);
    }

    void mattack1()
    {
        return;
    }


    // TESTING For debugging
    Vector2 hitbox_pos = new Vector2(0,0);
    Vector2 hitbox_size = new Vector2(0,0);
    IEnumerator TwoHit()
    {
        // Damage values
        float first_hit_dmg = 20f;
        float second_hit_dmg = 30f;

        // Helper vars
        int current_face = facing;
        Vector3 boss_size = GetComponent<BoxCollider2D>().size;
        Vector2 corner = new Vector2(transform.position.x + (current_face * boss_size.x/2), transform.position.y - boss_size.y/2);


        // Delay before first attack
        yield return new WaitForSeconds(2F); 

        // Set size of first attack here. Don't change hitbox_pos
        hitbox_size = new Vector2(2F,1F); 
        hitbox_pos = new Vector2(current_face * (hitbox_size.x + boss_size.x)/2, (hitbox_size.y - boss_size.y)/2);
        
        GameObject first_hit = Instantiate(melee_attack_prefab, transform);
        MeleeAttackHitBox box = first_hit.GetComponent<MeleeAttackHitBox>();
        if (box != null){
            box.LoadVars(hitbox_pos, hitbox_size, first_hit_dmg, 2);
        }

        // Delay before second attack
        yield return new WaitForSeconds(2F); 

        // Set size of first attack here. Don't change hitbox_pos
        hitbox_size = new Vector2(2F,1F); 
        hitbox_pos = new Vector2(current_face * (hitbox_size.x + boss_size.x)/2, (hitbox_size.y - boss_size.y)/2);
        
        GameObject second_hit = Instantiate(melee_attack_prefab, transform);
        box = second_hit.GetComponent<MeleeAttackHitBox>();
        if (box != null){
            box.LoadVars(hitbox_pos, hitbox_size, first_hit_dmg, 2);
        }

        
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

    void OnDrawGizmos() {
        // Vector3 hitbox_size = new Vector3(5,4,0);
        // Vector3 hitbox_pos = transform.position; //+ new Vector3(-2, boss.transform.h,0);
        // Gizmos.DrawWireCube(hitbox_pos, hitbox_size);
        return;
    }
}