using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    private float leftEdge;
    private float rightEdge;
    [SerializeField] RectTransform healthbar;
    [SerializeField] RectTransform white;

    private void Awake()
    {
        leftEdge = healthbar.offsetMin.x;
        rightEdge = healthbar.offsetMax.x;
    }

    public void ChangeHealth(float healthpercent)
    {
        float distance = rightEdge - leftEdge;
        float scaledHealth = distance * healthpercent;
        Vector3 healthvec = healthbar.offsetMax;
        healthvec.x += scaledHealth;
        healthbar.offsetMax = healthvec;

        Vector3 whitevec = white.anchoredPosition;
        whitevec.x += scaledHealth;
        white.anchoredPosition = whitevec;
    }
}
