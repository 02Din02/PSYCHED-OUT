using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitions : MonoBehaviour
{
    public static SceneTransitions Instance;
    [SerializeField] private Image image;
    // Assign as child in the inspector :D
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // Destroy the duplicate instance
        }
        else
        {
            Instance = this; // Set this as the instance
            DontDestroyOnLoad(gameObject); // Keep this instance alive across scenes
        }
    }


    public void FancyChangeSceneAlt(int sceneNum)
    // This is what they call a "Wrapper Method"
    {
        StartCoroutine(FancyChangeScene(sceneNum));
    }



    public IEnumerator FancyChangeScene(int targetScene)
    // Call this when you want to load a scene, find the number of the target scene in build settings
    {

        yield return image.DOFade(1, 1.5f).WaitForCompletion();

        SceneManager.LoadScene(targetScene);

        yield return image.DOFade(0, 4.5f).WaitForCompletion();

         transform.DOKill();
    }
}