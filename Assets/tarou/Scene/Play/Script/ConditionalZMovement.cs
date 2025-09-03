using UnityEngine;
using System.Collections;

public class ConditionalZMovement : MonoBehaviour
{
    public float moveSpeed = 1f;  // 通常の移動スピード（Z方向）
    public float addZ = 10f;      // 5秒後に加算するZ座標の量
    public float addZDuration = 2f; // 加算移動にかける時間（秒）

    private float startZ;         // 最初のZ座標
    private bool isWaiting = false; // 停止中フラグ
    public GameObject Danger;  // DangerUIを参照する

    void Start()
    {
        startZ = transform.position.z;

        Danger = GameObject.FindGameObjectWithTag("DangerUI");

        if (Danger != null)
        {
            Danger.SetActive(false); // 最初は非表示
        }
        else
        {
            Debug.LogWarning("DangerUIタグを持つオブジェクトが見つかりません");
        }
    }

    void Update()
    {
        if (isWaiting) return;

        // 初期位置より後ろにいるときだけ移動
        if (transform.position.z < startZ)
        {
            Vector3 pos = transform.position;
            pos.z += moveSpeed * Time.deltaTime;
            transform.position = pos;
        }

        // Zが -60 より低くなったら停止処理
        if (transform.position.z < -60f && !isWaiting)
        {
            StartCoroutine(StopAndAddZ());
        }
    }

    private IEnumerator StopAndAddZ()
    {
        isWaiting = true;

        if (Danger != null) Danger.SetActive(true); // DangerUI表示

        yield return new WaitForSeconds(5f); // その場で5秒静止

        // スーっと加算移動
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0f, 0f, addZ);

        while (elapsed < addZDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / addZDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos; // 最終位置を保証

        if (Danger != null) Danger.SetActive(false); // DangerUI非表示

        isWaiting = false; // 再び動けるように
    }
}