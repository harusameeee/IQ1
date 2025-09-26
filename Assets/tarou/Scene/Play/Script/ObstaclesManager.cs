using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{
    [Header("障害物として出現させるPrefab")]
    public GameObject obstaclePrefab;

    [Header("障害物の最大数")]
    public int maxObstacles = 10;

    [Header("障害物が出現するx座標の範囲")]
    public float minX = -5f;
    public float maxX = 5f;

    [Header("障害物出現間隔(秒)のランダム範囲")]
    public float minSpawnTime = 2f;
    public float maxSpawnTime = 5f;

    private List<GameObject> obstacles = new List<GameObject>();
    private float timer = 0f;
    private float nextSpawnTime = 0f;

    void Start()
    {
        SetNextSpawnTime();
    }


    void Update()
    {
        if (obstacles.Count < maxObstacles)
        {
            timer += Time.deltaTime;
            if (timer >= nextSpawnTime)
            {
                SpawnObstacle();
                timer = 0f;
                SetNextSpawnTime();
            }
        }
    }

    void SpawnObstacle()
    {
        // xを範囲内でランダムに
        float x = Random.Range(minX, maxX);
        // スポーン位置は空オブジェクトの位置を基準にローカルオフセット
        Vector3 spawnPos = this.transform.position + new Vector3(x, 0f, 0f);
        GameObject obj = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
        obstacles.Add(obj);
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }
}