using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public class OpticPillar : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private PlayerMovement player;
    public bool debug;

    int damage = 50;
    float width = 5.0F;
    float height = 10.0F;
    float attack_delay = 2F;
    float duration = 1.5F;
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
            script.TakeDamage(damage);
            boxCollider.enabled = false;
        }
    }

    void Start()
    {
        StartCoroutine(Activate());
    }

    public IEnumerator Activate(){
        boxCollider.size = new Vector2(width, height);
        // boxCollider.offset = new Vector2(0, height/2);

        boxCollider.enabled = false;

        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

        yield return new WaitForSeconds(attack_delay);

        boxCollider.enabled = true;

        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }
}
