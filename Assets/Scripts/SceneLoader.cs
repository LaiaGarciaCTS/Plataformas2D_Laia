using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] private GameObject _loadingCanvas;
    [SerializeField] private Image _loadingBar;

    private bool _isLoading = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(string sceneName)
    {
        if (_isLoading) return;

        StartCoroutine(LoadNewScene(sceneName));
    }

    
    private IEnumerator LoadNewScene(string sceneName)
    {
        _isLoading = true;

        _loadingCanvas.SetActive(true);
        _loadingBar.fillAmount = 0f;

        yield return null;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        if (asyncLoad == null)
        {
            Debug.LogError("No se encuentra la escena: " + sceneName);
            _loadingCanvas.SetActive(false);
            _isLoading = false;
            yield break;
        }

        asyncLoad.allowSceneActivation = false;

        float fakeLoadPercentage = 0f;

        // Llenar la barra falsa
        while (fakeLoadPercentage < 1f)
        {
            fakeLoadPercentage += Time.unscaledDeltaTime * 0.5f;
            fakeLoadPercentage = Mathf.Clamp01(fakeLoadPercentage);

            _loadingBar.fillAmount = fakeLoadPercentage;

            // Esperar a que la escena esté preparada y la barra llena
            if (fakeLoadPercentage >= 1f && asyncLoad.progress >= 0.9f)
            {
                break;
            }

            yield return null;
        }

        // Activar SampleScene
        asyncLoad.allowSceneActivation = true;

        // Esperar hasta que Unity termine de cargarla
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // IMPORTANTE: ocultar el canvas persistente
        _loadingCanvas.SetActive(false);

        Time.timeScale = 1f;
        _isLoading = false;

        Debug.Log("Escena activa: " + SceneManager.GetActiveScene().name);
    }
}