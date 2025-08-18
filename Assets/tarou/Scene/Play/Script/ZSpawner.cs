using UnityEngine;

public class ZSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnInterval = 2f;
    public Transform spawnPoint;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
            Debug.Log("Spawned!");
            timer = 0f;
        }
    }
}
