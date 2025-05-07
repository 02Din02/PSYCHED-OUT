using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas; // The Pause Menu, KEEP DE-ACTIVATED IN INSPECTOR!

    // Volume Slider
    public Slider volumeSlider;
    public TextMeshProUGUI volumeNumText;
    

    // Input
    [SerializeField] private InputActionReference pauseAction;


    void Awake()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed += Pause;
            pauseAction.action.Enable();
        }

        Time.timeScale = 1; // Un-Freeze time
    }
    public void Pause(InputAction.CallbackContext ctx)
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(true); // Show menu
            Cursor.lockState = CursorLockMode.None; // Free the mouse!!
            Time.timeScale = 0; // Freeze time
        }

    }

    public void UnPause()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false); // Get rid of menu
            //Cursor.lockState = CursorLockMode.Locked; // Locks mouse in place
            Time.timeScale = 1; // Un-Freeze time
        }

    }

    public void FullScreen(bool inFullScreen) //Toggles fullscreen from button press
    {
        Screen.fullScreen = inFullScreen;
    }

    public void Resolution(int dropdown)
    //toggles screen resoulation, gets the dropdown int from UI Dropdown
    // If we add more resoluations, MAKE THEM IN ORDER FROM LARGE TO SMALL!!! (16:9)
    {

        int widith = 1920;
        int height = 1080;
        //Defualt resoluton

        if (dropdown == 0)
        {
            widith = 1920;
            height = 1080;
        }
        else if (dropdown == 1)
        {
            widith = 1366;
            height = 768;
        }
        else if (dropdown == 2)
        {
            widith = 1280;
            height = 720;
        }
        else if (dropdown == 3)
        {
            widith = 10;
            height = 10;
        }

        Screen.SetResolution(widith, height, Screen.fullScreen);
    }

    public void MainMenu() // Activated by Button
    {
        SceneManager.LoadScene(0);
    }

    public void Volume(float volumeNum) //Activated by slider
    {
        AudioListener.volume = volumeNum / 100f;
        volumeNumText.text = volumeNum.ToString();
    }

}
