using UnityEngine;

public class TreasureSpawner : MonoBehaviour
{
    public GameObject treasurePrefab;
    public int numberOfTreasures = 10;
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;

    void Start()
    {
        for (int i = 0; i < numberOfTreasures; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                0f,
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            Instantiate(treasurePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
