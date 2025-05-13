using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.Mathematics;
using UnityEngine.Scripting.APIUpdating;
using UnityEditor;
using System;
using DG.Tweening;
using TMPro;

public class BossController : MonoBehaviour
{
    //gameobjects
    private PlayerMovement player;
    private BoxCollider2D boxCollider;
    new private Rigidbody2D rigidbody2D;
    private Animator bossAnim;
    private SpriteRenderer spriteRenderer;
    private SetupScript setupScript;

    //prefabs
    private GameObject laser_orb_prefab;
    private GameObject optic_pillar_prefab;
    private GameObject melee_attack_prefab;

    private GameObject shockwave_prefab;
    private AudioManager audioM;
    [SerializeField] private Material hurtMaterial;
    [SerializeField] private Material normalMaterial;
    private bool isPlaying = false;

    //ranges (P____[L]____lrange____[M]____mrange____[C]____B)
    float mrange = 7F;
    float crange = 5F;
    
    // UI
    public Slider healthSlider;
    public TextMeshProUGUI currency;

    // Stats
    public float maxhealth;
    public float health;
    private float move_duration = 5F;
    private float step_back_duration = 1F;
    private float step_back_speed = 5F;
    private float move_speed = 2F;
    private float step_back_chance = 0.25F; //out of 1
    private float pause_duration = 2F;
    private bool attacking = false;
    public int facing = -1; //1 for right, -1 for left

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        bossAnim = GetComponent<Animator>();
        setupScript = FindObjectOfType<SetupScript>();
        audioM = FindObjectOfType<AudioManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        laser_orb_prefab = Resources.Load("Laser_Orb") as GameObject;
        optic_pillar_prefab = Resources.Load("Optic_Pillar") as GameObject;
        melee_attack_prefab = Resources.Load("Melee_Attack") as GameObject;
        shockwave_prefab = Resources.Load("Shockwave") as GameObject;

        healthSlider.GetComponent<Slider>();

        // set the health on the boss health bar
        maxhealth = health;
        healthSlider.maxValue = health;
        healthSlider.value = health;
        curr_move_speed = move_speed;

        //setupScript.FadeOut(8);
    }
    void attack(float dist)
    {
        bossAnim.ResetTrigger("turn");
        //movesets for each range
        string[] lset = {"laser_orb", "optic_pillar"}; //"optic_pillar", "shockwave_slash"
        string[] mset = {"three_hit"};
        string[] cset = {"axe_slam", "three_hit", "two_hit"}; //"axe_slam", "three_hit", "two_hit"

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

        // Debug.Log(a);
        StartCoroutine(a, 0);
    }

    IEnumerator laser_orb()
    {
        bossAnim.ResetTrigger("turn");
        float animation_delay = 0.5F;
        bossAnim.SetTrigger("laserOrb");
        yield return new WaitForSeconds(animation_delay);

        float orb_dist = 4F; //distance from boss
        float orb_spread = 2F; //distance between each orb
        float orb_height = 1F; //the height of the middle orb
        float laser_spawn_delay = 0.1F; //delay between each orb spawn

        Vector3 pos_1 = new Vector3(orb_dist * facing, orb_height + orb_spread, 0) + transform.position;
        Vector3 pos_2 = new Vector3(orb_dist * facing, orb_height, 0) + transform.position;
        Vector3 pos_3 = new Vector3(orb_dist * facing, orb_height - orb_spread, 0) + transform.position;

        yield return new WaitForSeconds(laser_spawn_delay);
        Instantiate(laser_orb_prefab, pos_1, Quaternion.identity);
        audioM.PlaySound(audioM.laserChargeSFX);


        yield return new WaitForSeconds(laser_spawn_delay);
        Instantiate(laser_orb_prefab, pos_2, Quaternion.identity);
        audioM.PlaySound(audioM.laserChargeSFX);

        yield return new WaitForSeconds(laser_spawn_delay);
        Instantiate(laser_orb_prefab, pos_3, Quaternion.identity);
        audioM.PlaySound(audioM.laserChargeSFX);

        yield return new WaitForSeconds(pause_duration);
        audioM.PlaySound(audioM.laserReleaseSFX);
        
        attacking = false;

        yield return new WaitForSeconds(1);
        
    }

    IEnumerator optic_pillar()
    {
        bossAnim.ResetTrigger("turn");
        bossAnim.SetTrigger("opticPillar");
        float animation_delay = 1F;
        audioM.PlaySound(audioM.pillarChargeSFX);
        yield return new WaitForSeconds(animation_delay);
        Instantiate(optic_pillar_prefab, transform.position, Quaternion.identity);
        audioM.PlaySound(audioM.pillarReleaseSFX);
        yield return new WaitForSeconds(pause_duration);
        
        attacking = false;
    }

    IEnumerator three_hit()
    {
        bossAnim.ResetTrigger("turn");
        bossAnim.SetTrigger("threeHit");

        audioM.PlaySound(audioM.threeHitSFX);

        // Attack values
        int[] damage = {15, 30, 30}; 
        Vector2[] sizes = {new Vector2(1.35F,1F), new Vector2(1.7F,1F), new Vector2(4.5F,1F)}; 
        float[] delay = {0.7F,0.7F,0.5F}; //No initial delay
        float[] duration = {0.3F,0.2F,0.2F}; 

        // Helper vars
        int current_face = -1;
        Vector3 boss_size = GetComponent<BoxCollider2D>().size;
        Vector2 corner = new Vector2(transform.position.x + (current_face * boss_size.x/2), transform.position.y - boss_size.y/2.5f);

        for (int i = 0; i < 3; i ++){
            // Delay before attack
            yield return new WaitForSeconds(delay[i]);
            
            Vector2 pos = new Vector2(current_face * (sizes[i].x + boss_size.x)/2 + 0.5f, (sizes[i].y - boss_size.y)/2.5f - 0.5f);

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
        bossAnim.ResetTrigger("turn");
        bossAnim.SetTrigger("twoHit");
        audioM.PlaySound(audioM.twoHitSFX);

        // Attack values
        int[] damage = {20, 30}; 
        Vector2[] sizes = {new Vector2(1.7F,0.5F), new Vector2(0F,0F)};  //Vector2(1.5F,1F)
        float[] delay = {0.7F,0.5F}; 
        float[] duration = {0.3F,0.5F}; 

        // Helper vars
        int current_face = -1;
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
                    new Vector2(current_face * (sizes[i].x + boss_size.x)/2 +0.5f, (sizes[i].y - boss_size.y)/2), 
                    sizes[i], 
                    damage[i], 
                    duration[i]);
            }
        }

        yield return new WaitForSeconds(duration[1] + pause_duration);
        attacking = false;
    }

    IEnumerator axe_slam()
    {
        bossAnim.ResetTrigger("turn");
        bossAnim.SetTrigger("downwardAxe");
        audioM.PlaySound(audioM.downwardSFX);

        // Attack values
        int[] damage = {30}; 
        Vector2[] sizes = {new Vector2(2F,1F)};
        float[] delay = {0.7F}; 
        float[] duration = {0.2F};

        // Helper vars
        int current_face = -1;
        Vector3 boss_size = GetComponent<BoxCollider2D>().size;
        Vector2 corner = new Vector2(transform.position.x + (current_face * boss_size.x/2), transform.position.y - boss_size.y/2);

        for (int i = 0; i < 1; i ++){
            // Delay before attack
            yield return new WaitForSeconds(delay[i]); 
            
            // Set size of first attack here. Don't change hitbox_pos
            GameObject hit = Instantiate(melee_attack_prefab, transform);
            MeleeAttackHitBox box = hit.GetComponent<MeleeAttackHitBox>();
            if (box != null){
                box.LoadVars(
                    new Vector2(current_face * (sizes[i].x + boss_size.x)/2 +0.5f, (sizes[i].y - boss_size.y)/2), 
                    sizes[i], 
                    damage[i], 
                    duration[i]);
            }
        }

        yield return new WaitForSeconds(duration[0] + pause_duration);
        attacking = false;
    }

    IEnumerator shockwave_slash()
    {
        bossAnim.ResetTrigger("turn");
        bossAnim.SetTrigger("twoHit");
        audioM.PlaySound(audioM.upwardSFX);

        // Attack values
        int[] damage = {40}; 
        Vector2[] sizes = {new Vector2(1F,0.5F)};
        float[] delay = {0.7F}; 
        float[] duration = {0.2F};

        // Helper vars
        int current_face = -1;
        Vector3 boss_size = GetComponent<BoxCollider2D>().size;
        Vector2 corner = new Vector2(transform.position.x + (current_face * boss_size.x/2), transform.position.y - boss_size.y/2);

        for (int i = 0; i < 1; i ++){
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
            GameObject shockwave = Instantiate(shockwave_prefab, transform.position, Quaternion.identity);
            Shockwave shockwave_script = shockwave.GetComponent<Shockwave>();
            shockwave_script.facing = facing;
            
        }

        yield return new WaitForSeconds(duration[0] + pause_duration);

        attacking = false;
    }

    private float curr_move_time = 0F;
    private bool step_back_decision = true; //can still decide to step back
    private int move_dir = 1;
    private float curr_move_speed;
    void Update()
    {
        
        rigidbody2D.velocity = Vector2.zero;
        gameObject.transform.localScale = new Vector3(-3 * facing, 3, 1);

        if (attacking) {
            return;
        }
        
        // Updates direction that the boss should face
        float dist_from_player = player.transform.position.x - transform.position.x;
        if (dist_from_player > 0) {
           // if (facing == -1)
            //{
                //bossAnim.SetTrigger("turn");
           // }
            facing = 1;
        }
        else {
           // if (facing == 1)
           // {
            //    bossAnim.SetTrigger("turn");
           // }
            facing = -1;
        }

        if (!attacking) {
            float step_back_distance = step_back_speed * step_back_duration;
            RaycastHit2D ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left * facing, step_back_distance + 1, LayerMask.GetMask("Ground"));

            if (!ray) {
                if (((curr_move_time > move_duration - step_back_duration) || math.abs(dist_from_player) <= crange) && step_back_decision) {
                    if (UnityEngine.Random.Range(0F,1F) <= step_back_chance) {
                        move_dir = -1;
                        curr_move_speed = step_back_speed;
                        curr_move_time = move_duration - step_back_duration;
                    }
                    step_back_decision = false;
                }
            }

            if (curr_move_time > move_duration || (math.abs(dist_from_player) <= crange && move_dir == 1)) {
                attack(dist_from_player);
                curr_move_time = 0F;
                step_back_decision = true;
                move_dir = 1;
                curr_move_speed = move_speed;
                return;
            }

            rigidbody2D.velocity = new Vector2(move_dir * facing * curr_move_speed, 0);
            StartCoroutine(walkSFX());
            curr_move_time += Time.deltaTime;
        }
    }

    private IEnumerator walkSFX()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            yield return new WaitForSeconds(0.5f);
            audioM.PlaySound(audioM.bossWalk);
            isPlaying = false;
        }
    }

    void UpdateHealthBar()
    {
        // Should get called every time Player hits boss, NOT IN UPDATE!!!!!!
        healthSlider.value = health;
        float googoogaga = ((maxhealth - health) / maxhealth) * 100f;
        currency.text = "= $" + googoogaga.ToString("0");
    }

    public void take_damage(float damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject);
            audioM.PlaySound(audioM.bossDeath);
            setupScript.FadeIn();
        } else {
            UpdateHealthBar();
            audioM.PlaySound(audioM.bossGetHitSFX);
            StartCoroutine(Ouch());
        }
    }

    IEnumerator Ouch()
    {
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = hurtMaterial;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.material = normalMaterial;
    }
}