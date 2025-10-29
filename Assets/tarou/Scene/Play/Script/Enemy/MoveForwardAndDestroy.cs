using UnityEngine;

public class MoveForwardAndDestroy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float destroycountlimit = 5f;

    private float destroycount = 0f;

    private Transform followTarget;
    private Vector3 initialOffset;     // ターゲットとの初期距離
    private Quaternion initialRotation; // ターゲットとの初期回転差

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
        if (target != null)
        {
            initialOffset = transform.position - target.position;
            initialRotation = Quaternion.Inverse(target.rotation) * transform.rotation;
        }
    }

    void Update()
    {
        // 自分のforward方向に進む
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);

        if (followTarget != null)
        {
            // ターゲットに対して初期オフセットを維持する形で追従
            transform.position = followTarget.position + followTarget.rotation * initialOffset;
            transform.rotation = followTarget.rotation * initialRotation;
        }

        // 一定時間経過で破棄
        destroycount += Time.deltaTime;
        if (destroycount >= destroycountlimit)
        {
            Destroy(gameObject);
        }
    }
}
