using UnityEngine;

public class FallingObjectSpawner : MonoBehaviour
{
    [Header("落下させるオブジェクト")]
    public GameObject fallingPrefab; // 落下物のPrefab

    [Header("落下Y座標")]
    public float spawnY = 10f;

    [Header("生成間隔（秒）")]
    public float interval = 1.5f;

    void Start()
    {
        // intervalごとにSpawnFallingObjectを呼ぶ
        InvokeRepeating(nameof(SpawnFallingObject), 0f, interval);
    }

    void SpawnFallingObject()
    {
        // この空オブジェクトの位置・スケールを基準に落下範囲を決定
        float centerX = transform.position.x;
        float width = transform.localScale.x;

        float minX = centerX - width / 2f;
        float maxX = centerX + width / 2f;

        float x = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(x, spawnY, transform.position.z);

        Instantiate(fallingPrefab, spawnPos, Quaternion.identity);
    }
}