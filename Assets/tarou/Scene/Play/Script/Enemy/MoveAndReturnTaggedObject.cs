using UnityEngine;
using System.Collections;

public class MoveAndReturnTaggedObject : MonoBehaviour
{
    public string targetTag = "EnemyCharacter";
    public float moveDistance = -1f;
    public float moveDuration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
            foreach (GameObject target in targets)
            {
                // ターゲットに移動用コルーチンを走らせる
                target.AddComponent<MoveHelper>().StartMoving(moveDistance, moveDuration);
            }

            // ★ 自分は当たった瞬間に消す
            //Destroy(gameObject);
        }
    }
}

// ターゲット側に一時的に追加される補助クラス
public class MoveHelper : MonoBehaviour
{
    public void StartMoving(float distance, float duration)
    {
        StartCoroutine(MoveOverTime(distance, duration));
    }

    private IEnumerator MoveOverTime(float distance, float duration)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, 0, distance);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        // コルーチン終わったら補助コンポーネント削除
        Destroy(this); 
    }
}
