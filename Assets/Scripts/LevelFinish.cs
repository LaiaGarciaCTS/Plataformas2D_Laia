using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    private bool finished = false;

    //Campo para GameOver:
    [SerializeField] private int requiredStars = 3;
    [SerializeField] private string nextLevelScene = "Level1";

    private bool levelCompleted = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (finished || !other.CompareTag("Player"))
            return;

        finished = true;

        Victory victory = FindObjectOfType<Victory>();

        if (victory != null)
        {
            victory.ShowVictory();
        }
        else
        {
            Debug.LogError("No se encuentra el script Victory.");
        }



        if (!other.CompareTag("Player") || levelCompleted)
            return;

        if (GameManager.Instance.GetStarCount() >= requiredStars)
        {
            levelCompleted = true;
            SceneManager.LoadScene(nextLevelScene);
        }
        else
        {
            Debug.Log("Necesitas recoger las 3 estrellas para avanzar.");
        }
    }

}