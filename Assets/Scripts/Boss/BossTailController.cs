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
    private bool is_slowed = false;
    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        playerRB = player.GetComponent<Rigidbody2D>();
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !is_slowed) 
        {
            player.Stats.BaseSpeed *= slow_percent;
            Debug.Log(player.Stats.BaseSpeed + " Enter");
            is_slowed = true;
        }
        Debug.Log(player.Stats.BaseSpeed);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            exit_count += 1;
            StartCoroutine(delaySlow());
            Debug.Log(exit_count + " " + player.Stats.BaseSpeed + " Exit");
        }
    }
    public IEnumerator delaySlow()
    {
        yield return new WaitForSeconds(3);
        if (exit_count == 1){
            player.Stats.BaseSpeed /= slow_percent;
            Debug.Log(exit_count + " " + player.Stats.BaseSpeed + " Decrease");
            is_slowed = false;
        }
        exit_count -= 1;
    }

}
