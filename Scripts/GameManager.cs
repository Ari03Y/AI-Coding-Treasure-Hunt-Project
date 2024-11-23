using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int treasuresCollected = 0;
    public int totalTreasures = 10;
    public bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Time.timeScale = 1f; // Ensure timeScale is set to 1 at the start
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void CollectTreasure()
    {
        treasuresCollected = treasuresCollected + 1;
        UIManager.Instance.UpdateTreasureCount(treasuresCollected, totalTreasures);

        if (treasuresCollected >= totalTreasures)
        {
            EscapeDoor.Instance.UnlockDoor();
        }
    }

    public void GameOver(bool won)
    {
        isGameOver = true;
        Time.timeScale = 0f;
        EndMenuController.Instance.ShowEndMenu(won);
    }
}
