using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionScript : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        GetComponent<Image>().enabled = true;
        animator = GetComponent<Animator>();
        Time.timeScale = 1;
    }

    public void FadeOut()
    {
        animator.Play("FadeOut");
    }

    public void LoadLevel()
    {
        if(SceneManager.GetActiveScene().name == "Full Arena")
        {
            SceneManager.LoadScene("Upgrade Scene");
        }
        else if (SceneManager.GetActiveScene().name == "Upgrade Scene")
        {
            SceneManager.LoadScene("Full Arena");
        }

    }
}
