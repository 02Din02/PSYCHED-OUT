using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackHitBox : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private float damage;
    private float lifetime;
    private bool start_timer = false;

    public void LoadVars(Vector2 center, Vector2 size, float damage1, float lifetime1 = 5F){
        boxCollider = GetComponent<BoxCollider2D>();
        
        boxCollider.offset = center;
        boxCollider.size = size;
        damage = damage1;
        lifetime = lifetime1;

        start_timer = true;
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
        Debug.Log("Do Damage");
        boxCollider.enabled = false;
        Destroy(gameObject);
    }
}
