using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTailController : MonoBehaviour
{

    private PlayerMovement player;
    private Rigidbody2D playerRB;

    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        playerRB = player.GetComponent<Rigidbody2D>();

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            PlayerManager script = player.GetComponent<PlayerManager>();
        }
    }

    public IEnumerator slowPlayer()
    {
        playerRB.drag = 1;
        yield return new WaitForSeconds(3);
        playerRB.drag = 0;
    }

}
