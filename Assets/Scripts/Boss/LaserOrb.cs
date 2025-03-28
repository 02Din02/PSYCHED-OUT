using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserOrb : MonoBehaviour
{
    float attack_delay = 1F;
    float velocity = 5F;
    BoxCollider2D boxCollider =  ;
    Vector3 direction;
    public bool debug;

    private PlayerMovement player;

    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
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

    public IEnumerator Attack()
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
