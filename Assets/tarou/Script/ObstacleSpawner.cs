using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObstacleData
{
    public GameObject prefab;                        // 出す障害物のプレハブ
    public Vector2 xOffsetRange = new Vector2(-3f, 3f);       // スポナー位置からのX方向ランダム範囲
    public Vector2 spawnIntervalRange = new Vector2(2f, 4f);  // 出現間隔の範囲
}

public class ObstacleSpawner : MonoBehaviour
{
    public List<ObstacleData> obstacles; // 複数の障害物設定を持つ

    private float timer;
    private float nextSpawnTime;
    private ObstacleData currentObstacle;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            SpawnObstacle();
            ScheduleNextSpawn();
        }
    }

    void ScheduleNextSpawn()
    {
        timer = 0f;

        // ランダムに1つの障害物設定を選ぶ
        currentObstacle = obstacles[Random.Range(0, obstacles.Count)];

        // プレハブごとの間隔から次のスポーン時間を決定
        nextSpawnTime = Random.Range(currentObstacle.spawnIntervalRange.x, currentObstacle.spawnIntervalRange.y);
    }

    void SpawnObstacle()
    {
        if (currentObstacle == null || currentObstacle.prefab == null) return;

        // スポナーの位置を基準に、X方向だけランダムにずらす
        float offsetX = Random.Range(currentObstacle.xOffsetRange.x, currentObstacle.xOffsetRange.y);
        Vector3 spawnPos = transform.position + new Vector3(offsetX, 0f, 0f);

        // プレハブを生成
        Instantiate(currentObstacle.prefab, spawnPos, Quaternion.identity);
    }
}
