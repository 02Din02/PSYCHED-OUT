using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTailController : MonoBehaviour
{

    private PlayerMovement player;
    private Rigidbody2D playerRB;
    private float slow_percent = 0.3F;
    private int exit_count = 0;
    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        playerRB = player.GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && exit_count == 0) 
        {
            player.Stats.BaseSpeed *= slow_percent;
            Debug.Log("Slowed");
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            exit_count += 1;
            StartCoroutine(delaySlow());
        }
    }

    // Wasn't sure what this did. Drag didn't seem to slow when I ran it at least. Could be wrong.
    public IEnumerator delaySlow()
    {
        yield return new WaitForSeconds(3);
        if (exit_count == 1){
            player.Stats.BaseSpeed /= slow_percent;
            
            Debug.Log("Unslowed");
        }
        exit_count -= 1;
    }

}
