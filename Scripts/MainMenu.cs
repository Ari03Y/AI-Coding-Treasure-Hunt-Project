using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("DemoScene_night");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
