using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    private bool finished = false;

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
    }
}