using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTailController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ENTER TAIL");
    }

    void OnTriggerExit2D(Collider2D collision) 
    {
        Debug.Log("EXIT TAIL");
    }
}
