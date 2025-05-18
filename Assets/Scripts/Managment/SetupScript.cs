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
    private PlayerManager player;
    [SerializeField] DataManager dataManager;
    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
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

    public void Win()
    {
        if (fadeBox != null)
        {
            float fadeDuration = 3f;
            fadeBox.DOFade(1f, fadeDuration);
            StartCoroutine(winscene(fadeDuration));
        }
    }

    public IEnumerator reloadScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(1);
    }

     public IEnumerator winscene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(2);
    }

    public void FadeOut(int time)
    {
        if (fadeBox != null)
        {
            fadeBox.DOKill();
            fadeBox.color = new Color(0, 0, 0, 1f);
            fadeBox.DOFade(0f, time);
        }

    }

    public void FadeInWithoutRestarting(int fadeDuration)
    {
        if (fadeBox != null)
        {
            fadeBox.DOFade(1f, fadeDuration);
        }
    }



    public void UpgradesDone()
    {
        upgradeCanvas.gameObject.SetActive(false);

        FadeOut(5);
    }
}
