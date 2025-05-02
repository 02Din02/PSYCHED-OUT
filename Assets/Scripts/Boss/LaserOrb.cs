using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserOrb : MonoBehaviour
{
    int damage = 15;
    float attack_delay = 1.5F;
    float velocity = 20F;
    private BoxCollider2D boxCollider;
    private PlayerMovement player;
    private Vector3 direction;
    public bool debug;

    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        if (direction != null)
        {
            transform.position += direction * Time.deltaTime * velocity;
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attack_delay);

        Vector3 target_position = player.transform.position;
        direction = (target_position - transform.position).normalized;
        direction.y += 0.1f;
    }

    void OnDrawGizmos() {
        if (debug){
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(boxCollider.offset + new Vector2(transform.position.x,transform.position.y) , boxCollider.size);
        }
        return;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            PlayerManager script = player.GetComponent<PlayerManager>();
             if (player._rolling == false)
             {
                script.TakeDamage(damage, new Vector2(direction.x * velocity * 0.5f ,10));
                boxCollider.enabled = false;
             }
            
        }

        Destroy(gameObject);
    }
}
