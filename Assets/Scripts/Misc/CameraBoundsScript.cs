using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBoundsScript : MonoBehaviour
{
    [SerializeField] PolygonCollider2D confinerShape2D; // Drag the zone�s collider here (self)
    [SerializeField] GameObject player;
    [SerializeField] GameObject boss;
    [SerializeField] Cinemachine.CinemachineVirtualCamera virtualCam;

    //private void FixedUpdate()
    //{
    //    Debug.Log(Camera.main.name);
    //    //Debug.Log(player.transform.position.y);
    //    if (player.transform.position.y < -51f)
    //    {
    //        var camConfiner = virtualCam.GetComponentInChildren<Cinemachine.CinemachineConfiner2D>();
    //        camConfiner.m_BoundingShape2D = confinerShape2D;
    //        Debug.Log("Camera switched");
    //    }
    //}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure your player has the "Player" tag
        {
            var camConfiner = virtualCam.GetComponentInChildren<Cinemachine.CinemachineConfiner2D>();
            camConfiner.m_BoundingShape2D = confinerShape2D;
            boss.gameObject.SetActive(true);
            player.GetComponent<PlayerManager>().resetHealth();
            //Debug.Log("Camera switched");
        }
    }
}
