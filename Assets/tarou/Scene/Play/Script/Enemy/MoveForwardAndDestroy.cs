using UnityEngine;

public class MoveForwardAndDestroy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float destroycountlimit = 5f;

    private float destroycount = 0f;

    private Transform followTarget;  // 出した元（例：spawnPoint）
    private Vector3 lastTargetPos;   // 前フレームの位置
    private Quaternion lastTargetRot; // 前フレームの回転

    // 呼び出し元からターゲットをセット
    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
        if (target != null)
        {
            lastTargetPos = target.position;
            lastTargetRot = target.rotation;
        }
    }

    void Update()
    {
        // 自分の forward 方向に進む（ローカル空間で）
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);

        if (followTarget != null)
        {
            // --- 位置の差分を加算 ---
            Vector3 offset = followTarget.position - lastTargetPos;
            transform.position += offset;

            // --- 回転の差分を加算 ---
            // 回転の「差分」をクォータニオンで計算
            Quaternion deltaRot = followTarget.rotation * Quaternion.Inverse(lastTargetRot);
            transform.rotation = deltaRot * transform.rotation;

            // 状態を更新
            lastTargetPos = followTarget.position;
            lastTargetRot = followTarget.rotation;
        }

        // 一定時間経過で破棄
        destroycount += Time.deltaTime;
        if (destroycount >= destroycountlimit)
        {
            Destroy(gameObject);
        }
    }
}
