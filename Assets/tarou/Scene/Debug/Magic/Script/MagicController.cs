using UnityEngine;

[System.Serializable]
public class MagicSpawnData
{
    [Range(0f, 1f)]
    public float triggerPoint;   // 発動タイミング（0〜1）
    [Tooltip("magicPrefabs配列のインデックス番号（0〜）")]
    public int prefabIndex;      // 出す魔法の番号
    [Tooltip("spawnPoints配列のインデックス番号（0〜）")]
    public int spawnIndex;       // 出す場所の番号
    [HideInInspector] public bool triggered = false;
}

public class MagicController : MonoBehaviour
{
    [Header("魔法プレハブリスト（番号で指定）")]
    public GameObject[] magicPrefabs;  // 例: Fire, Ice, Wind など

    [Header("出現位置（ここに5か所登録）")]
    public Transform[] spawnPoints;    // 出現位置（例: Point0〜Point4）

    [Header("魔法発動リスト（タイミング＋どの魔法＋どの位置）")]
    public MagicSpawnData[] spawnList;

    private witch_mover moveLoop;

    void Start()
    {
        // MoveLoopを自動取得
        moveLoop = FindObjectOfType<witch_mover>();
        if (moveLoop == null)
            Debug.LogError(" witch_mover がシーン内に見つかりません");
    }

    void Update()
    {
        if (moveLoop == null || spawnList == null || spawnList.Length == 0)
            return;

        float t = moveLoop.current_t_normalized;

        foreach (var data in spawnList)
        {
            if (!data.triggered && t >= data.triggerPoint)
            {
                data.triggered = true;
                SpawnMagic(data);
            }
        }
    }

    private void SpawnMagic(MagicSpawnData data)
    {
        if (magicPrefabs == null || data.prefabIndex < 0 || data.prefabIndex >= magicPrefabs.Length)
            return;

        GameObject prefab = magicPrefabs[data.prefabIndex];
        if (prefab == null)
            return;

        // 出す位置を取得
        Vector3 pos;
        Quaternion rot;

        if (spawnPoints != null && data.spawnIndex >= 0 && data.spawnIndex < spawnPoints.Length)
        {
            Transform point = spawnPoints[data.spawnIndex];

            // スプラインの親に合わせたワールド位置・回転を使う
            pos = point.position;
            rot = point.rotation;
        }
        else
        {
            pos = transform.position;
            rot = transform.rotation;
        }

        //スプラインの向きに沿って生成
        Instantiate(prefab, pos, rot);
    }

    // MoveLoop が再スタートした時に呼ぶ（手動 or 自動）
    public void ResetTriggers()
    {
        foreach (var data in spawnList)
            data.triggered = false;
    }
}
