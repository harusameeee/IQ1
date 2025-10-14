using UnityEngine;
using UnityEngine.UI;

public class DistanceFromEnemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform witch;
    private Slider slider;
    private float distance;

    void Start()
    {
        // スライダーを取得
        slider = this.GetComponent<Slider>();

        // 初期距離を測る
        distance = Vector3.Distance(player.position, witch.position);

        // 最大値を設定
        slider.maxValue = distance;
    }

    void Update()
    {
        // 現在距離を測る
        distance = Vector3.Distance(player.position, witch.position);

        // スライダーに反映
        slider.value = distance;
    }
}
