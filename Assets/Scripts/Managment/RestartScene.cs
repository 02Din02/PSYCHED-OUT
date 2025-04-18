using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public SpriteRenderer fadeBox;
    public Canvas upgradeCanvas;
    public TextMeshProUGUI currencyText;
    [SerializeField] DataManager dataManager;
    void Start()
    { 
        upgradeCanvas.gameObject.SetActive(true);
        dataManager = FindObjectOfType<DataManager>();
        currencyText.text = dataManager.currency.ToString();
    }

    public void FadeIn()
    {   
        if (fadeBox != null)
        {
            fadeBox.DOFade(1f, 3);
            StartCoroutine(reloadScene());
        }
        
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
            fadeBox.DOFade(0f, 5);
        }
        
    }

    public void UpgradesDone()
    {
        upgradeCanvas.gameObject.SetActive(false);
        FadeOut();
    }
}
