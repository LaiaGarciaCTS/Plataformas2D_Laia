using UnityEngine;

public class CanvasManager : MonoBehaviour
{
   [SerializeField]private GameObject _pauseCanvas;

   public static CanvasManager Instance;

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

   public void ChangeCanvasStatus()
   {
        if(_pauseCanvas.activeInHierarchy)
        {
            _pauseCanvas.SetActive(false);
        }
        else
        {
            _pauseCanvas.SetActive(true);
        }
   }
}
