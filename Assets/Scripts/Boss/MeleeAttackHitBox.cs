using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MeleeAttackHitBox : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private PlayerMovement player;

    private BossController boss;
    private int damage;
    private float lifetime;
    private bool start_timer = false;

    private int curr_facing;

    public void LoadVars(Vector2 center, Vector2 size, int damage1, float lifetime1 = 5F){
        boxCollider = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<PlayerMovement>();
        boss = FindAnyObjectByType<BossController>();
        
        boxCollider.offset = center;
        boxCollider.size = size;
        damage = damage1;
        lifetime = lifetime1;

        start_timer = true;

        curr_facing = boss.facing;
    }

    void Update()
    {
        if (start_timer){
            lifetime -= Time.deltaTime;
            if (lifetime < 0){
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            PlayerManager script = player.GetComponent<PlayerManager>();
            if (player._rolling == false)
            {
                
            script.TakeDamage(damage, new Vector2(15 * curr_facing, 10));
            boxCollider.enabled = false;
            }
            
        }
    }
}
