using System;
using System.Collections.Generic;
using Unity.Mathematics;
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
        public enemypattern magicPattern; // 使用する魔法パターン（未使用）
    }
    
    protected static Random rng = new();
    public witch_mover mover; // 移動経路制御クラスへの参照 / Reference to the movement controller

    public Vector2 dim; // 当たり判定の大きさ / Hitbox dimensions
    public override Vector2 dimension => dim;

    public override Vector2 position =>
        new Vector2(-transform.localPosition.x, transform.localPosition.y + 8f);
    // 表示・判定上の位置補正 / Position offset for display or collision

    public static Action<float, List<damagable_type>, Vector2> enemyhit;
    public Transform spellcastpoint; // 魔法発動位置 / Magic casting position
    // 被弾時に通知するイベント / Event called when this enemy takes damage
    public int currentmagicindex = 0; // 現在の魔法発動インデックス / Current magic spawn index
    public int queuedspellindex = -1; // キューに入っている魔法発動インデックス / Queued spell index
    public Vector2 targetpos; // 目標位置（未使用） / Target position (unused)
    public float move_duration = 2.0f; // 移動にかかる時間（未使用） / Duration of movement (unused)
    public float move_speed= 4.0f; // 移動速度（未使用） / Movement speed (unused)
    public List<MagicSpawnData> magicspawns = new List<MagicSpawnData>(); // 魔法発動データリスト / List of magic spawn data
    public Animator animator; // アニメーター参照 / Reference to the animator
    [Header("random pattern mode")]
    
    public bool randompattern = false; // ランダムパターン使用フラグ / Flag to use random patterns
    public enemypattern currentrandompattern; // 現在のランダムパターン（未使用） / Current random pattern (unused)
    public List<enemypattern> spellpatterns = new List<enemypattern>(); // 使用する魔法パターンリスト / List of spell patterns to use
    public List<enemypattern> usedpatterns = new List<enemypattern>(); // 既に使用したパターンリスト / List of already used patterns
    public override void Start()
    {
        base.Start();
        Debug.Log($"Witch Ai2 Start called for {name}");
        AnimatorStateController.activespell += SpawnMagic;
        onspawn?.Invoke(this);
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
        if (comboable)
        {
            StartCoroutine(dmgflash());
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
        this.transform.localPosition = Vector3.Lerp(
            this.transform.localPosition,
            new Vector3(targetpos.x, targetpos.y, 0),
            Time.deltaTime * move_speed);
        if (move_duration > 0)
        {
            move_duration -= Time.deltaTime;
        }
        else
        {
            targetpos = Vector2.zero;
        }
        float t = mover.current_t_normalized;
        if (randompattern)
        {
            if(spellpatterns.Count == 0)
            {
                // すべてのパターンを使用済みに移動
                spellpatterns.AddRange(usedpatterns);
                var count = spellpatterns.Count;
                var last = count - 1;
                for (var i = 0; i < last; ++i) {
                    var r = UnityEngine.Random.Range(i, count);
                    var tmp = spellpatterns[i];
                    spellpatterns[i] = spellpatterns[r];
                    spellpatterns[r] = tmp;
                 }
                usedpatterns.Clear();
                Debug.Log("All patterns used, resetting used patterns.");
            }
            {
                var pattern = spellpatterns[0];
                spellpatterns.RemoveAt(0);
                usedpatterns.Add(pattern);
                queuedspellindex = -1; // Clear queued spell index
                currentrandompattern = pattern;
                targetpos = pattern.position;
                move_duration = pattern.duration;
                animator.Play("spellcast");
            }
        }
        else if(currentmagicindex < magicspawns.Count)
        {
            var magicdata = magicspawns[currentmagicindex];
            if (t >= magicdata.triggerPoint)
            {
                queuedspellindex = currentmagicindex;
                targetpos = magicspawns[currentmagicindex].magicPattern.position;
                move_duration = magicspawns[currentmagicindex].magicPattern.duration;
                currentmagicindex++;
                animator.Play("spellcast");
            }
        }
    }

    private void SpawnMagic()
    {
        if(queuedspellindex < 0 || queuedspellindex >= magicspawns.Count) return;
        if(randompattern)
        {
            if(currentrandompattern == null) return;
            foreach (var pattern in currentrandompattern.patterndata)
            {
                var temp = Instantiate(pattern.attackhbobj, spellcastpoint);
                temp.transform.localPosition = pattern.offset;
            }
            return;
        }
        var data = magicspawns[queuedspellindex];
        foreach (var pattern in data.magicPattern.patterndata)
        {
            var temp = Instantiate(pattern.attackhbobj, spellcastpoint);
            temp.transform.localPosition = pattern.offset;
        }
        queuedspellindex = -1;
    }
    // MoveLoop再スタート時に呼び出し（発動済みリセット）
    // Called when the movement loop restarts (resets triggered states)

}
