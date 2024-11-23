using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndMenuController : MonoBehaviour
{
    public static EndMenuController Instance;

    public GameObject endMenu;
    public TMP_Text endMessage;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ShowEndMenu(bool won)
    {
        if (endMenu != null)
        {
            if (won)
                endMessage.text = "You Escaped!";
            else
                endMessage.text = "You Lost! Try Again?";

            endMenu.SetActive(true);
        }
        else
        {
            Debug.LogWarning("EndMenu or EndMessage is not assigned in the EndMenuController.");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
