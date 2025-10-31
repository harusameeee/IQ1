using UnityEngine;

public class MoveForwardAndDestroy : MonoBehaviour
{
    public float moveSpeed = 17f;

    private Transform followTarget;
    private Vector3 initialOffset;
    private Quaternion initialRotation;

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

        // ターゲットに追従
        if (followTarget != null)
        {
            transform.position = followTarget.position + followTarget.rotation * initialOffset;
            transform.rotation = followTarget.rotation * initialRotation;
        }
    }
}
