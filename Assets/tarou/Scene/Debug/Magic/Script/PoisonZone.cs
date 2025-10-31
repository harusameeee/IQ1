using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoisonZone : MonoBehaviour
{
    [SerializeField] private float zoneDuration = 3f;    // ゾーンが続く時間
    [SerializeField] private float damageAmount = 10f;   // 1回あたりのダメージ量
    [SerializeField] private float damageInterval = 1f;  // 何秒ごとにダメージを与えるか

    private MoveForwardAndDestroy root;
    private bool isStarted = false;

    // Zoneに入っているプレイヤーを記録
    private readonly HashSet<entity> playersInZone = new HashSet<entity>();

    public void SetRoot(MoveForwardAndDestroy rootObject)
    {
        root = rootObject;
    }

    public void StartZone()
    {
        if (isStarted) return;
        isStarted = true;
        StartCoroutine(ZoneProcess());
        StartCoroutine(DamageLoop());
    }

    private IEnumerator ZoneProcess()
    {
        Debug.Log("Zone開始");

        // 一定時間ゾーンを維持
        yield return new WaitForSeconds(zoneDuration);

        Debug.Log("Zone終了 → Root削除");

        if (root != null)
        {
            Destroy(root.gameObject); // おおもとごと削除
        }
        else
        {
            Destroy(gameObject); // 念のため自分だけでも消す
        }
    }

    private IEnumerator DamageLoop()
    {
        while (isStarted)
        {
            foreach (var player in playersInZone)
            {
                if (player != null)
                {
                    player.TakeDamage(
                        damageAmount,
                        true,      // comboable
                        null,      // damagable_Types（指定なし）
                        player.transform.position // ヒット位置
                    );

                    Debug.Log($"{player.name} took {damageAmount} poison damage from Zone!");
                }
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }

    // プレイヤーが入った時
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            var entity = other.GetComponent<entity>();
            if (entity != null && !playersInZone.Contains(entity))
            {
                playersInZone.Add(entity);
            }
        }
    }

    // プレイヤーが出た時
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            var entity = other.GetComponent<entity>();
            if (entity != null && playersInZone.Contains(entity))
            {
                playersInZone.Remove(entity);
            }
        }
    }
}
