using UnityEngine;

public class HomingObject : MonoBehaviour
{
    public float speed = 10f;  // 飛ぶ速度
    public float destroyZ = -20f;

    private Transform target;

    void Start()
    {
        // "Player"と"Player2"タグがついたオブジェクトを探す
        GameObject player1 = GameObject.FindGameObjectWithTag("Player");
        GameObject player2 = GameObject.FindGameObjectWithTag("Player2");

        // どちらかが存在すれば、ランダムでターゲットにする
        GameObject[] candidates = new GameObject[] { player1, player2 };
        candidates = System.Array.FindAll(candidates, go => go != null);

        if (candidates.Length > 0)
        {
            GameObject chosen = candidates[Random.Range(0, candidates.Length)];
            target = chosen.transform;

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