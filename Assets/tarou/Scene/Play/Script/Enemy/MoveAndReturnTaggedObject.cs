using UnityEngine;
using System.Collections;

public class MoveAndReturnTaggedObject : MonoBehaviour
{
    public string targetTag = "EnemyCharacter"; // 動かしたいタグ
    public float moveDistance = -1f;             // 進める距離
    public float moveDuration = 2f;             // 移動にかける時間（秒）

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
            foreach (GameObject target in targets)
            {
                StartCoroutine(MoveOverTime(target, moveDistance, moveDuration));
            }
        }
    }

    private IEnumerator MoveOverTime(GameObject target, float distance, float duration)
    {
        Vector3 startPos = target.transform.position;
        Vector3 endPos = startPos + new Vector3(0, 0, distance);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            target.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.transform.position = endPos; // 最終位置を保証
    }
}