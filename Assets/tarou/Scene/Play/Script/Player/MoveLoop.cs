using UnityEngine;

public class MoveLoop : MonoBehaviour
{
    [Header("移動設定")]
    public float speed = 2.0f; // 通常の移動速度
    public Vector3 direction = new Vector3(0, 0, 1); // 奥(Z)方向に動く

    [Header("プレイヤー参照")]
    public Player3DController player; // Player3DController参照用
    public Player3DController player2; // Player3DController参照用

    private float defaultSpeed; // 初期速度保存用

    void Start()
    {
        defaultSpeed = speed;

        // Inspector未設定なら自動取得（例: プレイヤーに"Player"タグが付いている場合）
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.GetComponent<Player3DController>();
        }
    }

    void Update()
    {
        if ((player != null && player.currentState == Player3DController.State.Slow) ||
            (player2 != null && player2.currentState == Player3DController.State.Slow))
        {
            speed = 1.5f;
        }
        else
        {
            speed = defaultSpeed;
        }
        // 移動処理
        transform.Translate(direction * speed * Time.deltaTime);
    }
}