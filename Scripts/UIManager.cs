using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text healthText;
    public TMP_Text treasureText;
    public GameObject mathProblemPanel;
    public TMP_Text problemText;
    public TMP_InputField answerInput;
    public TMP_Text mathProblemTimerText;
    public GameObject[] jumpscareImages;
    public Slider healthBar;
    public AudioClip jumpscareSound;

    private bool isAnswerCorrect = false;
    private bool answerSubmitted = false;

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

        // Initialize UI elements
        UpdateHealth(100);
        UpdateTreasureCount(0, 10);
    }

    public void UpdateHealth(int health)
    {
        if (healthText != null)
            healthText.text = $"HP: {health}";
        else
            Debug.LogWarning("HealthText is not assigned in the UIManager.");

        if (healthBar != null)
            healthBar.value = health;
        else
            Debug.LogWarning("HealthBar is not assigned in the UIManager.");
    }

    public void UpdateTreasureCount(int collected, int total)
    {
        if (treasureText != null)
            treasureText.text = $"Treasures: {collected}/{total}";
        else
            Debug.LogWarning("TreasureText is not assigned in the UIManager.");
    }

    public void ShowMathProblem(string problem, float timeLimit)
    {
        if (mathProblemPanel != null && problemText != null && answerInput != null)
        {
            mathProblemPanel.SetActive(true);
            problemText.text = problem;
            answerInput.text = "";
            answerSubmitted = false;
            answerInput.ActivateInputField();

            if (mathProblemTimerText != null)
                mathProblemTimerText.text = $"Time Left: {Mathf.Ceil(timeLimit)}s";
        }
        else
        {
            Debug.LogWarning("MathProblemPanel or its components are not assigned in the UIManager.");
        }
    }

    public IEnumerator WaitForAnswer(float timeLimit)
    {
        float elapsedTime = 0f;
        isAnswerCorrect = false;
        answerSubmitted = false;

        answerInput.onEndEdit.AddListener(OnAnswerSubmitted);

        while (!answerSubmitted && elapsedTime < timeLimit)
        {
            elapsedTime += Time.unscaledDeltaTime;

            if (mathProblemTimerText != null)
            {
                float timeRemaining = timeLimit - elapsedTime;
                mathProblemTimerText.text = $"Time Left: {Mathf.Ceil(timeRemaining)}s";
            }

            yield return null;
        }

        answerInput.onEndEdit.RemoveListener(OnAnswerSubmitted);
        mathProblemPanel.SetActive(false);
    }

    private void OnAnswerSubmitted(string input)
    {
        isAnswerCorrect = MathProblemGenerator.CheckAnswer(input);
        answerSubmitted = true;
    }

    public bool IsAnswerCorrect()
    {
        return isAnswerCorrect;
    }

    public void ShowJumpscare()
    {
        StartCoroutine(JumpscareCoroutine());
    }

    private IEnumerator JumpscareCoroutine()
    {
        int randomIndex = Random.Range(0, jumpscareImages.Length);
        GameObject selectedImage = jumpscareImages[randomIndex];
        AudioClip sound = jumpscareSound;

        selectedImage.SetActive(true);
        AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position, 1.0f);
        yield return new WaitForSecondsRealtime(0.8f);
        selectedImage.SetActive(false);
    }
}
