using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    /*
    para poner manualmente el nombre del nivel en el inspector, mas rapido para cunado haya mas de 1 nivel.
    public vois ChangeScene(string sceneName)
    {
        SceneManagement.LoadScene(sceneName)
    }
    */
    public void ChangeScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
}
