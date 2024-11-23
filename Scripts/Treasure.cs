using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour
{
    private bool isCollected = false;

    public AudioClip collectionSound;

    void OnTriggerEnter(Collider other)
    {
        if (isCollected)
            return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;
            StartCoroutine(SolveMathProblem());
        }
    }

    IEnumerator SolveMathProblem()
    {
        Time.timeScale = 0f;

        string problem = MathProblemGenerator.GenerateAlgebraProblem();
        UIManager.Instance.ShowMathProblem(problem, 20f);

        yield return UIManager.Instance.WaitForAnswer(20f);

        Time.timeScale = 1f;

        if (UIManager.Instance.IsAnswerCorrect())
        {
            AudioSource.PlayClipAtPoint(collectionSound, transform.position, 1.0f);

            PlayerController player = FindObjectOfType<PlayerController>();
            player.Heal(10);
            GameManager.Instance.CollectTreasure();
            Destroy(gameObject);
        }
        else
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            player.TakeDamage(10);
            UIManager.Instance.ShowJumpscare();
            isCollected = false;
        }
    }
}
