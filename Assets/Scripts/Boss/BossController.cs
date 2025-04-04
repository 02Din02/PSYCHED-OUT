using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Rendering.Universal;
using Unity.Collections;

public class BossController : MonoBehaviour
{
    //gameobjects
    private PlayerMovement player;
    private BoxCollider2D boxCollider;
    private GameObject laser_orb_prefab;
    private GameObject optic_pillar_prefab;
    private GameObject melee_attack_prefab;
    private Rigidbody2D rigidbody2D;

    // Stats
    public float bossHealth;
    
    // UI
    public Slider healthSlider;

    private int num_moves = 3;
    private int curr_moves = 0;
    private float move_time = 2F;
    private float move_speed = 0.5F;
    private int step_back_chance = 5; //out of 10
    private bool moving = false;
    private bool attacking = false;
    private bool finished_attacking = false;
    int facing = -1; //1 for right, -1 for left

    void Start()
    {
        // Find the player
        player = FindObjectOfType<PlayerMovement>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        // Find the player
        healthSlider.GetComponent<Slider>();
        // Get the Slider script of the boss health bar
        healthSlider.GetComponent<Slider>();

        // set the health on the boss health bar
        healthSlider.maxValue = bossHealth;
        healthSlider.value = bossHealth;
        // set the health on the boss health bar

        curr_moves = num_moves;
    }
    void attack(float dist)
    {
        //ranges (____[L]____lrange____[M]____mrange____[C]____)
        float lrange = 3F;
        float mrange = 2F;

        //movesets for each range
        string[] lset = {"laser_orb", "optic_pillar"};
        string[] mset = {"three_hit"};
        string[] cset = {"two_hit"};

        dist = Mathf.Abs(dist);

        attacking = true;

        string a;
        if (dist >= lrange) {
            a = lset[Random.Range(0, lset.Length)];
        }
        else if (dist >= mrange) {
            a = mset[Random.Range(0, mset.Length)];
        }
        else {
            a = cset[Random.Range(0, cset.Length)];
        }

        Invoke(a, 0);
    }

    // chance is out of 10
    IEnumerator move(bool step_back)
    {
        moving = true;

        int dir = facing;
        if (step_back) {
            dir *= -1;
        }

        rigidbody2D.velocity = new Vector2(dir * move_speed, 0);

        yield return new WaitForSeconds(move_time);

        rigidbody2D.velocity = Vector2.zero;

        moving = false;
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

        attacking = true;
    }

    void optic_pillar()
    {
        Instantiate(optic_pillar_prefab, transform.position, Quaternion.identity);

        finished_attacking = true;
    }

    IEnumerator ThreeHit()
    {
        // Attack values
        float[] damage = {15F, 30F, 30F}; 
        Vector2[] sizes = {new Vector2(2.5F,1F), new Vector2(2F,1F), new Vector2(4F,1F)}; 
        float[] delay = {0F,0.5F,0.7F}; //No initial delay
        float[] duration = {0.5F,0.5F,0.5F}; 

        // Helper vars
        int current_face = facing;
        Vector3 boss_size = GetComponent<BoxCollider2D>().size;
        Vector2 corner = new Vector2(transform.position.x + (current_face * boss_size.x/2), transform.position.y - boss_size.y/2);

        for (int i = 0; i < 3; i ++){
            // Delay before attack
            yield return new WaitForSeconds(delay[i]); 
            
            Vector2 pos = new Vector2(current_face * (sizes[i].x + boss_size.x)/2, (sizes[i].y - boss_size.y)/2);

            // Set size of first attack here. Don't change hitbox_pos
            GameObject hit = Instantiate(melee_attack_prefab, transform);
            MeleeAttackHitBox box = hit.GetComponent<MeleeAttackHitBox>();
            if (box != null){
                if (i == 2){
                    pos = new Vector2(0, pos.y);
                }

                box.LoadVars(
                    pos, 
                    sizes[i], 
                    damage[i], 
                    duration[i]);
            }


        }
    }

    IEnumerator TwoHit()
    {
        // Attack values
        float[] damage = {20F, 30F}; 
        Vector2[] sizes = {new Vector2(2F,1F), new Vector2(1.5F,1F)}; 
        float[] delay = {2F,2F}; 
        float[] duration = {2F,2F}; 

        // Helper vars
        int current_face = facing;
        Vector3 boss_size = GetComponent<BoxCollider2D>().size;
        Vector2 corner = new Vector2(transform.position.x + (current_face * boss_size.x/2), transform.position.y - boss_size.y/2);

        for (int i = 0; i < 2; i ++){
            // Delay before attack
            yield return new WaitForSeconds(delay[i]); 
            
            // Set size of first attack here. Don't change hitbox_pos
            GameObject hit = Instantiate(melee_attack_prefab, transform);
            MeleeAttackHitBox box = hit.GetComponent<MeleeAttackHitBox>();
            if (box != null){
                box.LoadVars(
                    new Vector2(current_face * (sizes[i].x + boss_size.x)/2, (sizes[i].y - boss_size.y)/2), 
                    sizes[i], 
                    damage[i], 
                    duration[i]);
            }
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
        if (moving) {
            return;
        }

        if (attacking) {
            if (finished_attacking) {
                curr_moves = num_moves;
                attacking = false;
                finished_attacking = false;
            }

            return;
        }

        // Updates direction that the boss should face
        float dist_from_player = player.transform.position.x - transform.position.x;
        if (dist_from_player > 0) {
            facing = 1;
        }
        else {
            facing = -1;
        }

        if (curr_moves == 0) {
            attack(dist_from_player);
        } else {
            int x = 11;
            if (curr_moves == 1) {
                x = Random.Range(0, 10) + 1;
            }

            if (x <= step_back_chance) {
                //move towards player
                move(true);
            } else {
                //move away from player
                move(false);
            }

            curr_moves--;
        }
    }
}