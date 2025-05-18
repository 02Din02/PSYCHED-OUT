using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHateCoding : MonoBehaviour
{
    private DataManager dataManager;
    void Start()
    {
        dataManager = FindObjectOfType<DataManager>();
    }

    public void Reset()
    {
        dataManager.Reset();
    }

    public void Cheat()
    {
        dataManager.Cheat();
    }

    // Update is called once per frame
    void Update()
    {
        dataManager = FindObjectOfType<DataManager>();
    }
}
