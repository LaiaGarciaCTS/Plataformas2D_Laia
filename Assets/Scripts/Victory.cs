
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    //Canvas de victoria:
    [SerializeField] private GameObject victoryCanvas;

    //Campo estrellas amarillas:
    [SerializeField] private GameObject[] yellowStars;

    //Campo botones:
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject retryButton;
    [SerializeField] private GameObject nextButton;

    //Campo nombres de las escenas:
    [SerializeField] private string menuSceneName = "MainMenu";
    [SerializeField] private string nextSceneName = "Level2";

    private bool victoryShown = false;

    private void Start()
    {
        victoryCanvas.SetActive(false);

        exitButton.SetActive(true);
        retryButton.SetActive(true);
        nextButton.SetActive(false);

        UpdateStars();
    }

    
    public void UpdateStars()
    {
        int collected = GameManager.Instance.GetStarCount();

        Debug.Log("Victory recibe " + collected + " estrellas.");

        for (int i = 0; i < yellowStars.Length; i++)
        {
            yellowStars[i].SetActive(i < collected);

            Debug.Log(
                "Estrella amarilla " + (i + 1) +
                ": " + yellowStars[i].activeSelf
            );
        }
    }


    public void ShowVictory()
    {
        if (victoryShown)
        {
            return;
        }

        victoryShown = true;

        UpdateStars();

        bool allStarsCollected = GameManager.Instance.GetStarCount() >= 3;

        victoryCanvas.SetActive(true);

        exitButton.SetActive(true);
        retryButton.SetActive(!allStarsCollected);
        nextButton.SetActive(allStarsCollected);

        Time.timeScale = 0f;
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;

        SceneLoader.Instance.ChangeScene(menuSceneName);
    }

    public void NextLevel()
    {
        if (GameManager.Instance.GetStarCount() < 3)
            return;

        Time.timeScale = 1f;

        SceneLoader.Instance.ChangeScene(nextSceneName);
    }
}