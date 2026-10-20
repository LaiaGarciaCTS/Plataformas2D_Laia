using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]private int coins;
    private bool _isPaused = false;

    [SerializeField]private int star;

    public Text monedaText;
    public Text estrellaText;

    public int coinCount = 0;
    public int starCount = 0;

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

}
