using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.Mathematics;

public class BossController : MonoBehaviour
{
    //gameobjects
    private PlayerMovement player;
    private BoxCollider2D boxCollider;
    new private Rigidbody2D rigidbody2D;

    //prefabs
    private GameObject laser_orb_prefab;
    private GameObject optic_pillar_prefab;
    private GameObject melee_attack_prefab;

    //ranges (P____[L]____lrange____[M]____mrange____[C]____B)
    float mrange = 10F;
    float crange = 5F;
    
    // UI
    public Slider healthSlider;

    // Stats
    public float bossHealth;
    private float move_duration = 4F;
    private float step_back_duration = 1F;
    private float move_speed = 1F;
    private float step_back_chance = 0.2F; //out of 1
    private float pause_duration = 1F;
    private bool attacking = false;
    int facing = -1; //1 for right, -1 for left

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        laser_orb_prefab = Resources.Load("Laser_Orb") as GameObject;
        optic_pillar_prefab = Resources.Load("Optic_Pillar") as GameObject;
        melee_attack_prefab = Resources.Load("Melee_Attack") as GameObject;

        healthSlider.GetComponent<Slider>();

        // set the health on the boss health bar
        healthSlider.maxValue = bossHealth;
        healthSlider.value = bossHealth;
    }
    void attack(float dist)
    {
        //movesets for each range
        string[] lset = {"laser_orb", "optic_pillar"};
        string[] mset = {"three_hit"};
        string[] cset = {"two_hit"};

        dist = Mathf.Abs(dist);

        attacking = true;

        string a;
        if (dist <= crange) {
            a = cset[UnityEngine.Random.Range(0, cset.Length)];
        }
        else if (dist <= mrange) {
            a = mset[UnityEngine.Random.Range(0, mset.Length)];
        }
        else {
            a = lset[UnityEngine.Random.Range(0, lset.Length)];
        }

        StartCoroutine(a, 0);
    }

    IEnumerator laser_orb()
    {
        float animation_delay = 1F;

        yield return new WaitForSeconds(animation_delay);

        float orb_dist = 2F; //distance from boss
        float orb_spread = 2F; //distance between each orb
        float orb_height = 1F; //the height of the middle orb
        float laser_spawn_delay = 0.3F; //delay between each orb spawn

        Vector3 pos_1 = new Vector3(orb_dist * facing, orb_height + orb_spread, 0) + transform.position;
        Vector3 pos_2 = new Vector3(orb_dist * facing, orb_height, 0) + transform.position;
        Vector3 pos_3 = new Vector3(orb_dist * facing, orb_height - orb_spread, 0) + transform.position;

        yield return new WaitForSeconds(laser_spawn_delay);
        Instantiate(laser_orb_prefab, pos_1, Quaternion.identity);

        yield return new WaitForSeconds(laser_spawn_delay);
        Instantiate(laser_orb_prefab, pos_2, Quaternion.identity);

        yield return new WaitForSeconds(laser_spawn_delay);
        Instantiate(laser_orb_prefab, pos_3, Quaternion.identity);

        yield return new WaitForSeconds(pause_duration);
        attacking = false;
    }

    IEnumerator optic_pillar()
    {
        float animation_delay = 1F;

        yield return new WaitForSeconds(animation_delay);

        Instantiate(optic_pillar_prefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(pause_duration);
        attacking = false;
    }

    IEnumerator three_hit()
    {
        float animation_delay = 1F;

        yield return new WaitForSeconds(animation_delay);
        
        // Attack values
        int[] damage = {15, 30, 30}; 
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

        yield return new WaitForSeconds(duration[2] + pause_duration);
        attacking = false;
    }

    IEnumerator two_hit()
    {
        float animation_delay = 1F;

        yield return new WaitForSeconds(animation_delay);

        // Attack values
        int[] damage = {20, 30}; 
        Vector2[] sizes = {new Vector2(2F,1F), new Vector2(1.5F,1F)}; 
        float[] delay = {0.5F,0.5F}; 
        float[] duration = {0.5F,0.5F}; 

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

        yield return new WaitForSeconds(duration[1] + pause_duration);
        attacking = false;
    }

    private float curr_move_time = 0F;
    private bool step_back_decision = true; //can still decide to step back
    private int move_dir = 1;
    void Update()
    {
        rigidbody2D.velocity = Vector2.zero;
        // Updates direction that the boss should face
        float dist_from_player = player.transform.position.x - transform.position.x;
        if (dist_from_player > 0) {
            facing = 1;
        }
        else {
            facing = -1;
        }

        if (attacking) {
            return;
        }

        if (!attacking) {
            if (curr_move_time > move_duration || math.abs(dist_from_player) <= crange) {
                attack(dist_from_player);
                curr_move_time = 0F;
                step_back_decision = true;
                move_dir = 1;
                return;
            }

            if ((curr_move_time > move_duration - step_back_duration) && step_back_decision) {
                if (UnityEngine.Random.Range(0F,1F) <= step_back_chance) {
                    move_dir = -1;
                }
                step_back_decision = false;
            }

            rigidbody2D.velocity = new Vector2(move_dir * facing * move_speed, 0);
            curr_move_time += Time.deltaTime;
        }
    }

    void UpdateHealthBar()
    {
        // Should get called every time Player hits boss, NOT IN UPDATE!!!!!!
        healthSlider.value = bossHealth;
    }

    void take_damage(float damage) {
        bossHealth -= damage;

        if (bossHealth <= 0) {
            Destroy(gameObject);
        } else {
            UpdateHealthBar();
        }
    }
}