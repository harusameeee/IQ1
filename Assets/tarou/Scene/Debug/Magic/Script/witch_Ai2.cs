using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class witch_Ai2 : entity
{
    [System.Serializable]
    public class MagicSpawnData
    {
        [Range(0f, 1f)]
        public float triggerPoint;   // 発動タイミング（0〜1）
        // Activation timing (0–1 normalized progress along the movement path)

        [Tooltip("magicPrefabs配列のインデックス番号（0〜）")]
        public int prefabIndex;      // 出す魔法の番号
        // Index number of the magic prefab to spawn (refers to magicPrefabs array)

        [Tooltip("spawnPoints配列のインデックス番号（0〜）")]
        public int spawnIndex;       // 出す位置の番号
        // Index number of the spawn position (refers to spawnPoints array)

        [HideInInspector] public bool triggered = false;
        // 発動済みかどうか
        // Whether this magic has already been triggered
    }

    protected static Random rng = new();
    public witch_mover mover; // 移動経路制御クラスへの参照 / Reference to the movement controller

    [Header("魔法プレハブリスト（番号で指定）")]
    // List of magic prefabs (specified by index in the inspector)
    public GameObject[] magicPrefabs;

    [Header("出現位置（ここに5か所登録）")]
    // Spawn points (register 5 or more locations here)
    public Transform[] spawnPoints;

    [Header("魔法発動リスト（タイミング＋どの魔法＋どの位置）")]
    // Magic spawn list (trigger timing + which magic + which spawn point)
    public MagicSpawnData[] spawnList;

    public Vector2 dim; // 当たり判定の大きさ / Hitbox dimensions
    public override Vector2 dimension => dim;

    public override Vector2 position =>
        new Vector2(transform.localPosition.x, transform.localPosition.y + 8f);
    // 表示・判定上の位置補正 / Position offset for display or collision

    public static Action<float, List<damagable_type>, Vector2> enemyhit;
    // 被弾時に通知するイベント / Event called when this enemy takes damage

    public override void Start()
    {
        base.Start();

        // moverが未設定なら自動取得
        // Auto-find the witch_mover component if not manually assigned
        if (mover == null)
            mover = FindObjectOfType<witch_mover>();
    }

    public override bool TakeDamage(float damageAmount, bool comboable = true,
        List<damagable_type> damagable_Types = null, Vector2 hitpoint = new Vector2())
    {
        float dmgmult = 1.0f;

        // バフを確認して被ダメージ倍率を計算
        // Check buffs for vulnerability multiplier
        foreach (var buff in buffs)
        {
            if (buff.type == bufftypes.vulnerability)
            {
                dmgmult += buff.pow;
            }
        }

        Debug.Log($"Witch took {damageAmount} damage with mult {dmgmult}");

        // ヒットイベント呼び出し
        // Invoke the on-hit event
        onHit?.Invoke((int)(damageAmount * dmgmult), comboable);

        // 少しランダムな揺れを与えたヒット座標
        // Add slight random offset to the hitpoint for visual variation
        float xOffset = rng.Next(-5, 5);
        float yOffset = rng.Next(-5, 5);
        hitpoint = new Vector2(hitpoint.x + xOffset / 5, hitpoint.y + yOffset / 5);

        // 敵ヒットイベントを通知
        // Notify any external listeners (e.g., damage effects)
        enemyhit?.Invoke((int)(damageAmount * dmgmult), damagable_Types, hitpoint);

        return true;
    }

    public override void Update()
    {
        base.Update();
        if (mover == null)
            return;

        // 現在の移動経路上の進行度（0〜1）
        // Current normalized progress along the movement path
        float t = mover.current_t_normalized;

        // --- 魔法発動処理 ---
        // --- Magic spawning process ---
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
        // 魔法プレハブとインデックスを確認
        // Validate prefab index and existence
        if (magicPrefabs == null || data.prefabIndex < 0 || data.prefabIndex >= magicPrefabs.Length)
            return;

        GameObject prefab = magicPrefabs[data.prefabIndex];
        if (prefab == null)
            return;

        Vector3 pos;
        Quaternion rot;

        // 出現位置を取得（指定があれば）
        // Get spawn position and rotation (if specified)
        if (spawnPoints != null && data.spawnIndex >= 0 && data.spawnIndex < spawnPoints.Length)
        {
            Transform point = spawnPoints[data.spawnIndex];
            pos = point.position;
            rot = point.rotation;
        }
        else
        {
            // 指定が無ければ自分の位置に生成
            // Default to witch’s current position
            pos = transform.position;
            rot = transform.rotation;
        }

        // 魔法を生成
        // Instantiate the magic prefab
        var temp = Instantiate(prefab, pos, rot);

        // moverのスプラインに沿った制御を引き継ぐ
        // Inherit the spline controller reference from the witch mover
        moving_obstacle mov = temp.GetComponent<moving_obstacle>();
        if (mov != null)
            mov.splinecont = mover.splinecont;
    }

    // MoveLoop再スタート時に呼び出し（発動済みリセット）
    // Called when the movement loop restarts (resets triggered states)
    public void ResetTriggers()
    {
        foreach (var data in spawnList)
            data.triggered = false;
    }
}
