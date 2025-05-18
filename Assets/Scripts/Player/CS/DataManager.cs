using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public int attemptNum;
    public int currency;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Reset()
    {
        attemptNum = 0;
        currency = 0;
        SceneManager.LoadScene(1);
    }

    public void Cheat()
    {
        attemptNum = 999;
        currency = 9999;
        SceneManager.LoadScene(1);
    }

}
