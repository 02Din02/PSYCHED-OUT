using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public SpriteRenderer fadeBox;
    public Canvas upgradeCanvas;
    void Start()
    { 
        upgradeCanvas.gameObject.SetActive(true);
    }

    public void FadeIn()
    {   
        if (fadeBox != null)
        {
            return;
        }
        fadeBox.DOKill();
        fadeBox.DOFade(1f, 5);
        StartCoroutine(reloadScene());
    }

    public IEnumerator reloadScene()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(1);
    }

    public void FadeOut()
    {   
        if (fadeBox != null)
        {
            return;
        }
        fadeBox.DOKill();
        fadeBox.DOFade(0f, 2);
    }
}
