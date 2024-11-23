using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    public float gameTime = 300f; // 5 minutes
    public TMP_Text timerText;

    private float elapsedTime = 0f;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Remove DontDestroyOnLoad if not needed
            // DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        UpdateTimerDisplay(gameTime - elapsedTime);
    }

    void Update()
    {
        if (isGameOver)
            return;

        elapsedTime += Time.deltaTime;
        float timeRemaining = gameTime - elapsedTime;
        UpdateTimerDisplay(timeRemaining);

        if (timeRemaining <= 0)
        {
            GameManager.Instance.GameOver(false);
            isGameOver = true;
        }
    }

    void UpdateTimerDisplay(float timeRemaining)
    {
        if (timerText != null)
            timerText.text = $"Time: {Mathf.Ceil(timeRemaining)}s";
        else
            Debug.LogWarning("TimerText is not assigned in the GameTimer.");
    }
}
