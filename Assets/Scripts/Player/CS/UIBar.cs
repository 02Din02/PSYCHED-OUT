using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIBar : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    private Vector2 initSize;
    private float leftEdge;
    private float rightEdge;

        // Start is called before the first frame update
        void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        //initSize = rectTransform.sizeDelta;
        leftEdge = rectTransform.offsetMin.x;
        rightEdge = rectTransform.offsetMax.x;
    }

    public void ChangeBar(float health)
    {
        rectTransform.offsetMax += new Vector2(Math.Min(400,(health - 100)), 0);
    }
}
