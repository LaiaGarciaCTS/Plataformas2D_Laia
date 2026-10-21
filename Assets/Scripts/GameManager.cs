using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool _isPaused = false;

    //contador de monedas, estrellas y corazones
    [SerializeField]private int star;
    [SerializeField]private int coins;
    [SerializeField]private int heart;

    public Text monedaText;
    public Text estrellaText;
    public Text corazonText;

    public int coinCount = 0;
    public int starCount = 0;
    public int heartCount = 0;

    //barra de vida
    [SerializeField]private GameObject _gameplayCanvas;
    [SerializeField]private Image _healthBar;

    //public float _playerHealth = 10;


    void Awake()
    {
        if(Instance!= null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        AudioManager.Instance.StartSoundtrack();
    }

    public void AddCoin()
    {
        coins += 1;
        monedaText.text = coins.ToString();
    }

    public void AddStar()
    {
        star += 1;
        estrellaText.text = star.ToString();
    }

    public void AddHeart()
    {
        heart += 1;
        corazonText.text = heart.ToString();
    }

    public void CoinCounter()
    {
        coins++;
        monedaText.text = coins.ToString();
    }

    public void StarCounter()
    {
        star++;
        estrellaText.text = star.ToString();
    }

    public void HeartCounter()
    {
        heart++;
        corazonText.text = star.ToString();
    }

    //barra de vida
    public IEnumerator ReduceHealthBar(float actualHeatlh)
    {
        float starHealth = _healthBar.fillAmount;
        _healthBar.fillAmount = starHealth - actualHeatlh;
        yield return new WaitForSecondsRealtime (1f);
    }

    public IEnumerator IncreaseHealthBar(float actualHealth)
    {
        float startHealth = _healthBar.fillAmount;
        _healthBar.fillAmount = startHealth + actualHealth;
        yield return new WaitForSecondsRealtime (1f);
    }


    //menu de pausa
    public void Pause()
    {
        if(_isPaused)
        {
            _isPaused = false;
            AudioManager.Instance.StartSoundtrack();
            Time.timeScale = 1;
        }
        else
        {
            _isPaused = true;
            AudioManager.Instance.StartSoundtrack();
            Time.timeScale = 0;
        }

        CanvasManager.Instance.ChangeCanvasStatus(CanvasManager.Instance.pauseCanvas, CanvasManager.Instance.resumeButton);
    }

    public bool IsPaused()
    {
        return _isPaused;
    }
}
