using UnityEngine;
using System.Collections.Generic; // damagable_type のために必要かも

public class FireMagic : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f; // 与えるダメージ量

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            TriggerEvent(other);
        }
    }

    private void TriggerEvent(Collider player)
    {
        HPEvent(player);
        Destroy(gameObject); // 魔法を削除
    }

    private void HPEvent(Collider player)
    {
        var playerEntity = player.GetComponent<entity>();
        if (playerEntity != null)
        {
            // すべての引数を正しい順番で渡す
            playerEntity.TakeDamage(
                damageAmount,               // ダメージ量
                true,                       // comboable（連携攻撃などで有効にしたいならtrue）
                null,                       // damagable_Types（指定なし）
                playerEntity.position        // ヒット位置（省略もOK）
            );

            Debug.Log($"{playerEntity.name} took {damageAmount} damage from FireMagic!");
        }
        else
        {
            Debug.LogWarning("当たったオブジェクトに entity スクリプトが見つかりません。");
        }
    }
}
