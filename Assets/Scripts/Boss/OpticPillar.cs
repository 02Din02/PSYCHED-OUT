using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public class OpticPillar : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public GameObject player;
    public bool debug;

    float width = 10.0F;
    float height = 10.0F;
    float attack_delay = 5;
    float duration = 1.5F;
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
    }

    void Start()
    {
        StartCoroutine(Activate());
        Debug.Log("START");
    }

    public IEnumerator Activate(){
        boxCollider.size = new Vector2(width, height);
        boxCollider.offset = new Vector2(0, height/2);

        boxCollider.enabled = false;

        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.y);

        yield return new WaitForSeconds(attack_delay);

        boxCollider.enabled = true;

        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
        
        // The boss makes an uppercutting motion with their fist, their hand visibly shaking. 
        // Afterwards, a black puddle the size of 10 players in width appears on the floor of where the player is standing,
        // looking like a black, shadowy mass with eyes in it. This puddle does nothing at first, but after 5 seconds, the puddle, 
        // after bubbling, will burst into an explosion containing black mass and eyes, having infinite vertical range and 
        // dealing massive damage, and existing for 1.5 seconds. The player can not jump over this attack, but can dodge or 
        // walk outside of the range. They can only be hit by this pillar once. 


    }

}
