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
        // Activation timing (0–1 normalized progress along the path)

        [Tooltip("magicPrefabs配列のインデックス番号（0〜）")]
        public int prefabIndex;      // 出す魔法の番号
        // Index of the magic prefab to spawn (refers to magicPrefabs array)

        [Tooltip("spawnPoints配列のインデックス番号（0〜）")]
        public int spawnIndex;       // 出す位置の番号
        // Index of the spawn position (refers to spawnPoints array)

        [HideInInspector] public bool triggered = false;
        // Whether this magic has already been triggered
    }

    protected static Random rng = new();
    public witch_mover mover;

    [Header("魔法プレハブリスト（番号で指定）")]
    // List of magic prefabs (specified by index)
    public GameObject[] magicPrefabs;

    [Header("出現位置（ここに5か所登録）")]
    // Spawn points (assign 5 locations here)
    public Transform[] spawnPoints;

    [Header("魔法発動リスト（タイミング＋どの魔法＋どの位置）")]
    // List of magic triggers (timing + which magic + which position)
    public MagicSpawnData[] spawnList;

    [Header("通常射撃設定")]
    // Normal shooting settings
    public GameObject projectile;
    public float shootInterval = 2.0f;
    private float shootTimer = 0.0f;
    public int maxdist = 5;
    public Vector2 dim;
    public override Vector2 dimension => dim;
    public override Vector2 position => new Vector2(transform.localPosition.x, transform.localPosition.y + 8f);

    public override void Start()
    {
        base.Start();
        if (mover == null)
            mover = FindObjectOfType<witch_mover>();
        // Auto-find the witch_mover if not assigned
    }

    public override bool TakeDamage(float damageAmount, bool comboable = true, List<damagable_type> damagable_Types = null)
    {
        float dmgmult = 1.0f;
        foreach (var buff in buffs)
        {
            if (buff.type == bufftypes.vulnerability)
                dmgmult += buff.pow;
        }

        Debug.Log($"Witch took {damageAmount} damage with mult {dmgmult}");
        onHit?.Invoke((int)(damageAmount * dmgmult), comboable);
        return true;
    }

    public override void Update()
    {
        base.Update();
        if (mover == null)
            return;

        float t = mover.current_t_normalized; // 0〜1 で経路上の進行度を取得
        // Get the normalized progress (0–1) along the movement path

        // --- 魔法発動処理 ---
        // --- Magic spawn trigger process ---
        foreach (var data in spawnList)
        {
            if (!data.triggered && t >= data.triggerPoint)
            {
                data.triggered = true;
                SpawnMagic(data);
            }
        }

        // --- 通常射撃（継続処理） ---
        // --- Normal continuous shooting ---
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0.0f;
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        int xOffset = rng.Next(-maxdist, maxdist);
        // Random horizontal offset for projectile spawn

        Vector3 spawnPos = mover.getobstaclespawnpos(0, 2.0f, out bool valid, out float new_t);
        // Get a valid spawn position from the mover along the spline

        if (!valid) return;

        var temp = Instantiate(projectile, spawnPos, Quaternion.identity);
        moving_obstacle mov = temp.GetComponent<moving_obstacle>();
        mov.splinecont = mover.splinecont;

        // Adjust horizontal position offset
        mov.obstacle.localPosition = new Vector3(
            xOffset,
            mov.obstacle.localPosition.y,
            mov.obstacle.localPosition.z
        );
    }

    private void SpawnMagic(MagicSpawnData data)
    {
        if (magicPrefabs == null || data.prefabIndex < 0 || data.prefabIndex >= magicPrefabs.Length)
            return;

        GameObject prefab = magicPrefabs[data.prefabIndex];
        if (prefab == null)
            return;

        Vector3 pos;
        Quaternion rot;

        if (spawnPoints != null && data.spawnIndex >= 0 && data.spawnIndex < spawnPoints.Length)
        {
            Transform point = spawnPoints[data.spawnIndex];
            pos = point.position;
            rot = point.rotation;
        }
        else
        {
            pos = transform.position;
            rot = transform.rotation;
        }

        // moverのスプラインに沿った生成処理（spline処理維持）
        // Instantiate along the mover’s spline, maintaining spline behavior
        var temp = Instantiate(prefab, pos, rot);
        moving_obstacle mov = temp.GetComponent<moving_obstacle>();
        if (mov != null)
            mov.splinecont = mover.splinecont;
    }

    // MoveLoop再スタート時に呼び出し
    // Called when the MoveLoop restarts (to reset trigger states)
    public void ResetTriggers()
    {
        foreach (var data in spawnList)
            data.triggered = false;
    }
}
