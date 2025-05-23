using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using System.Threading;

public class CameraBoundsScript : MonoBehaviour
{
    [SerializeField] PolygonCollider2D confinerShape2D; // Drag the zone�s collider here (self)
    [SerializeField] GameObject player;
    [SerializeField] GameObject boss;
    [SerializeField] GameObject playerLight;
    [SerializeField] GameObject bossMusic;
    [SerializeField] GameObject tutMusic;

    [SerializeField] GameObject bossHP_bar;
    [SerializeField] private SetupScript setupScript;
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
        if (other.CompareTag("Player"))
        {
            tutMusic.SetActive(false);
            bossMusic.SetActive(true);
            //Color c = setupScript.fadeBox.color;
            //c.a = 1f;
            //setupScript.fadeBox.color = c;

            StartCoroutine(switchCam());
            
            //Debug.Log("Camera switched");
        }
    }

    public IEnumerator switchCam()
    {
        setupScript.FadeInWithoutRestarting(1);
        yield return new WaitForSeconds(1);
        var camConfiner = virtualCam.GetComponentInChildren<Cinemachine.CinemachineConfiner2D>();
        camConfiner.m_BoundingShape2D = confinerShape2D;
        setupScript.FadeOut(1);
        yield return new WaitForSeconds(1);
        playerLight.SetActive(false);
        boss.gameObject.SetActive(true);
        bossHP_bar.gameObject.SetActive(true);
        player.GetComponent<PlayerManager>().ResetHealth();
        
    }
}



