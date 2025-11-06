using UnityEngine;
using Cysharp.Threading.Tasks;

public class CameraEventMagic : MonoBehaviour
{
    private ObstacleManager obstacleManager;

    [SerializeField] private float damageAmount = 10f; // 与えるダメージ量

    [System.Obsolete]
    private void Start()
    {
        // シーン上のObstacleManagerを探して参照保持
        obstacleManager = FindObjectOfType<ObstacleManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            TriggerEvent().Forget();
            HPEvent(other);
            Destroy(gameObject);
        }
    }

    private async UniTaskVoid TriggerEvent()
    {
        if (obstacleManager == null) return;

        // フラグをONにしてイベント開始
        obstacleManager.isActive = true;

        // 待機
        await UniTask.Delay(1500);

        // フラグをOFFにしてイベント終了
        obstacleManager.isActive = false;
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
