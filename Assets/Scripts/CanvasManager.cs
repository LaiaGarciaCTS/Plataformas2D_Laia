using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
   public GameObject pauseCanvas;

   public static CanvasManager Instance;

   public Button resumeButton;

   public GameObject gameOverCanvas;
   public Button retryButton;

   void Awake()
   {
        if(Instance !=null && Instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }
   }

   public void ChangeCanvasStatus(GameObject canvas, Button selectedButton)
   {
        if(canvas.activeInHierarchy)
        {
            canvas.SetActive(false);
        }
        else
        {
            canvas.SetActive(true);
            selectedButton.Select();
        }
   }
}
