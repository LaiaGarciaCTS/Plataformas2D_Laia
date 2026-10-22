using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [Header("Sonido")]
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _soundtrack;

    [Header("Botones")]
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject retryButton;

    [Header("Escenas")]
    [SerializeField] private string mainMenuScene = "Main Menu";
    [SerializeField] private string levelScene = "SampleScene";

    [Header("Estrellas amarillas")]
    [SerializeField] private GameObject[] yellowStars;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartSoundtrack();
        ShowStars();
    }

    void ShowStars()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("No se encuentra el GameManager.");
            return;
        }

        int starCount = GameManager.Instance.GetStarCount();
        starCount = Mathf.Clamp(starCount, 0, 3);

        Debug.Log("Estrellas recogidas: " + starCount);

        for (int i = 0; i < yellowStars.Length; i++)
        {
            if (yellowStars[i] != null)
            {
                yellowStars[i].SetActive(i < starCount);
            }
        }
    }

    public void StartSoundtrack()
    {
        if (_audioSource != null && _soundtrack != null)
        {
            _audioSource.clip = _soundtrack;
            _audioSource.Play();
        }
    }

    public void PauseSoundtrack()
    {
        if (_audioSource != null)
        {
            _audioSource.Pause();
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }
}