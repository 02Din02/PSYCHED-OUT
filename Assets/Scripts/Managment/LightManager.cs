using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light2D playerLight;
    private Transform player;
    private float dimLight;
    public float distance;
    void Start()
    {
        gameObject.SetActive(true);
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.position);

        if (distance > 35f)
        {
            playerLight.intensity = 0f;
        }
        else
        {
            playerLight.intensity = 1f - (distance / 30f); 
        }
    }
}
