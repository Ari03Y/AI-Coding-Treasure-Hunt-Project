using UnityEngine;
using System.Collections;

public class EscapeDoor : MonoBehaviour
{
    public static EscapeDoor Instance;

    private bool isUnlocked = false;
    private bool isSolved = false;

    void Awake()
    {
        Instance = this;
    }

    public void UnlockDoor()
    {
        isUnlocked = true;
        // Optional: Change door color to indicate it's unlocked
        GetComponent<Renderer>().material.color = Color.green;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isUnlocked || isSolved)
            return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(SolveCalculusProblem());
        }
    }

    IEnumerator SolveCalculusProblem()
    {
        Time.timeScale = 0f;

        string problem = MathProblemGenerator.GenerateCalculusProblem();
        UIManager.Instance.ShowMathProblem(problem, 30f);

        bool problemSolved = false;

        while (!problemSolved)
        {
            yield return UIManager.Instance.WaitForAnswer(30f);

            if (UIManager.Instance.IsAnswerCorrect())
            {
                isSolved = true;
                GameManager.Instance.GameOver(true);
                problemSolved = true;
            }
            else
            {
                PlayerController player = FindObjectOfType<PlayerController>();
                player.TakeDamage(15);
                UIManager.Instance.ShowJumpscare();

                if (player.health <= 0)
                {
                    problemSolved = true;
                }
                else
                {
                    problem = MathProblemGenerator.GenerateCalculusProblem();
                    UIManager.Instance.ShowMathProblem(problem, 30f);
                }
            }
        }

        Time.timeScale = 1f;
    }
}
