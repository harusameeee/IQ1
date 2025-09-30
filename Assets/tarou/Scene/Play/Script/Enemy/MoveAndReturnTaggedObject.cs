using UnityEngine;
using System.Collections;

// プレイヤーがトリガーに触れたとき、特定のタグのオブジェクトを動かして
// 元の位置に戻す処理を行うスクリプト
public class MoveAndReturnTaggedObject : MonoBehaviour
{
    public string targetTag = "EnemyCharacter"; // 動かす対象のタグ
    public float moveDistance = -1f;            // Z方向に動かす距離
    public float moveDuration = 2f;             // 移動にかかる時間

    private void OnTriggerEnter(Collider other)
    {
        // Player または Player2 がこのオブジェクトのトリガーに入ったとき
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            // 指定されたタグを持つオブジェクトをすべて取得
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

            foreach (GameObject target in targets)
            {
                // 各ターゲットに MoveHelper を動的に追加し、移動を開始する
                target.AddComponent<MoveHelper>().StartMoving(moveDistance, moveDuration);
            }

            // ★ 当たり判定した自分自身は消す場合
            //Destroy(gameObject);
        }
    }
}

// 移動処理を担当する補助コンポーネント
// 移動完了後に自動で削除される
public class MoveHelper : MonoBehaviour
{
    // 移動開始メソッド
    public void StartMoving(float distance, float duration)
    {
        // コルーチンで一定時間かけて移動させる
        StartCoroutine(MoveOverTime(distance, duration));
    }

    // 一定時間かけて対象を動かす処理
    private IEnumerator MoveOverTime(float distance, float duration)
    {
        Vector3 startPos = transform.position;                 // 移動開始位置
        Vector3 endPos = startPos + new Vector3(0, 0, distance); // 移動終了位置
        float elapsed = 0f;                                    // 経過時間

        // 指定時間が経つまでループ
        while (elapsed < duration)
        {
            // 開始位置から終了位置へ補間
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // 1フレーム待つ
        }

        // 最後に終了位置にぴったり合わせる
        transform.position = endPos;

        // 移動処理が終わったら、この補助コンポーネントを削除
        Destroy(this);
    }
}
