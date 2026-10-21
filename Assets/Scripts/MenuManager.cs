using UnityEngine;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.StartSoundtrack();
    }

    public void ChangeScene(string sceneName)
    {
        SceneLoader.Instance.ChangeScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}