using UnityEngine;
using Cysharp.Threading.Tasks; 

public class CameraEventMagic : MonoBehaviour
{
    private ObstacleManager obstacleManager;

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
}
