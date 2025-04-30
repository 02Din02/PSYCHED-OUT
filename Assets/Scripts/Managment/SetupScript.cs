using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupScript : MonoBehaviour
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
            float fadeDuration = 3f;
            fadeBox.DOFade(1f, fadeDuration);
            StartCoroutine(reloadScene(fadeDuration));
        }
    }

    public IEnumerator reloadScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(1);
    }

    public void FadeOut(int time)
    {

        if (fadeBox != null)
        {
            fadeBox.DOFade(0f, time);
        }

    }



    public void UpgradesDone()
    {
        upgradeCanvas.gameObject.SetActive(false);
        FadeOut(5);
    }
}
