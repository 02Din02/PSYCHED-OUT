using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserOrb : MonoBehaviour
{
    float damage = 15F;
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
        Debug.Log(direction);
        Debug.Log(velocity);
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
        Debug.Log("Do Damage");
        boxCollider.enabled = false;
        Destroy(gameObject);
    }
}
