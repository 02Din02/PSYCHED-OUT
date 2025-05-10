using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // KEEP ALL CANVASES BUT THE MAIN ONE NOT ACTIVATED!!
    public int gameScene; //Game Scene
    public GameObject settingsCanvas; //Canvas for settings :D
    public GameObject creditsCanvas; // Guess what this is

    public GameObject currentCanvas; //The canvas the player is on currently (used to close later)
    DataManager dataManager;

    void Start()
    {
        //dataManager = FindObjectOfType<DataManager>();
    }

    public void StartClicked()
    {
        //dataManager.attemptNum = 0;
        //dataManager.currency = 0;
        SceneManager.LoadScene(gameScene); // Called by clicking Start Button
    }

    public void CreditsClicked()
    {
        creditsCanvas.SetActive(true);
        currentCanvas = creditsCanvas;
        // When Credits button is clicked, show the canvas and set the current canvas
    }

    public void SettingsClicked()
    {
        settingsCanvas.SetActive(true);
        currentCanvas = settingsCanvas;
        // When Settings button is clicked, show the canvas and set the current canvas
    }

    public void CloseScreen()
    {
        currentCanvas.SetActive(false);
        // Called by exit buttons of settings & credit canvases
    }

    public void ExitClicked()
    {
        Application.Quit();
        Debug.Log("In theory, this would exit the game... I hope");
        //closes the game
    }

}
