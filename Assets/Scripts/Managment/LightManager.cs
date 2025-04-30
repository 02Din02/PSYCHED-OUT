using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light2D playerLight;
    private Transform player;
    private float dimLight;
    void Start()
    {
        gameObject.SetActive(true);
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        playerLight.intensity = 1f / (10f + distance);

        
    }
}
