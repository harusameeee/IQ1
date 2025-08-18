using UnityEngine;

public class HomingObject : MonoBehaviour
{
    public float speed = 10f;  // 飛ぶ速度
    public float destroyZ = -20f;

    private Transform target;

    void Start()
    {
        // "Player" タグがついたオブジェクトを探す
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;

            // Rigidbody に速度を設定してプレイヤーの方向に飛ばす
            Vector3 direction = (target.position - transform.position).normalized;
            GetComponent<Rigidbody>().velocity = direction * speed;
        }
        else
        {
            Debug.LogWarning("プレイヤーが見つかりませんでした");
        }
    }

    void Update()
    {
        // 画面外に行ったら破棄
        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }
}
