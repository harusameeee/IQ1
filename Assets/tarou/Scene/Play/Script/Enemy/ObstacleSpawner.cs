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
    public Transform basetransform; // スポナーの基準位置

    private float timer;
    private float nextSpawnTime;
    private ObstacleData currentObstacle;
    public bool localspace = false;
    private bool playerInside = false;  // ← Playerが中にいるかどうか判定するフラグ

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (playerInside) return; // ← プレイヤーが入ってる間は弾(障害物)を出さない

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
        Vector3 spawnPos = transform.position + transform.rotation*new Vector3(offsetX, 0f, 0f);

        // プレハブを生成
        if (localspace)
        {
            Instantiate(currentObstacle.prefab, spawnPos, Quaternion.identity, basetransform);
        }
        Instantiate(currentObstacle.prefab, spawnPos, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            playerInside = false;
        }
    }
}

