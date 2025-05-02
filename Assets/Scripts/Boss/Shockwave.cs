using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private PlayerMovement player;
    public bool debug;
    public int facing;

    int damage = 30;
    float width = 9.0F;
    float height = 2.0F;
    float duration = 10;
    float velocity = 20F;
    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        boxCollider = GetComponent<BoxCollider2D>();
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
            script.TakeDamage(damage);
            boxCollider.enabled = false;
            }
            
        }
    }

    void Start()
    {
        StartCoroutine(Activate());
    }

    public IEnumerator Activate() {
        boxCollider.size = new Vector2(width, height);
        transform.position = new Vector3(transform.position.x, -57.75F, transform.position.z);
        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }

    void Update() {
        if (facing != 0)
        {
            transform.position += new Vector3(facing, 0, 0) * Time.deltaTime * velocity;
        }
    }
}
