using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    /*
    para poner manualmente el nombre del nivel en el inspector, mas rapido para cunado haya mas de 1 nivel.
    public vois ChangeScene(string sceneName)
    {
        SceneManagement.LoadScene(sceneName)
    }
    */

    public static SceneLoader Instance;

    [SerializeField]private GameObject _loadingCanvas;
    [SerializeField]private Image _loadingBar;


    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }
    }


    /*public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }*/
    
    public void ChageScene(string sceneName)
    {
        StartCoroutine(LoadNewScene(sceneName));
    }

    IEnumerator LoadNewScene(string sceneName)
    {
        yield return null;
        _loadingCanvas.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        float fakeLoadPercentage = 0;

        while(!asyncLoad.isDone)
        {
            //_loadingBar.fillAmount = asyncLoad.progress;

            fakeLoadPercentage += 0.01f;
            Mathf.Clamp01(fakeLoadPercentage);

            _loadingBar.fillAmount = fakeLoadPercentage;

            if(asyncLoad.progress >= 0.09f && fakeLoadPercentage >= 0.99f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return new WaitForSecondsRealtime(0.1f);
        }

        Time.timeScale = 1;
        _loadingCanvas.SetActive(false);
    }
}
