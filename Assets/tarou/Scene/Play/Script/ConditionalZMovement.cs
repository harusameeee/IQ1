using UnityEngine;
using System.Collections;

public class ConditionalZMovement : MonoBehaviour
{
    public float moveSpeed = 1f;  // 移動スピード（Z方向）
    public float addZ = 10f;      // 5秒後に加算するZ座標の量

    private float startZ;         // 最初のZ座標
    private bool isWaiting = false; // 停止中フラグ

    void Start()
    {
        startZ = transform.position.z;
    }

    void Update()
    {
        if (isWaiting) return;

        // 現在位置が初期位置より後ろにいるときだけ移動
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

        // その場で5秒静止
        yield return new WaitForSeconds(5f);
        
        // Z座標を加算
        Vector3 pos = transform.position;
        pos.z += addZ;
        transform.position = pos;

        isWaiting = false; // フラグ解除して再び動けるようにする
    }
}
