using UnityEngine;

public class ConditionalZMovement : MonoBehaviour
{
    public float moveSpeed = 1f; // 移動スピード（Z方向）

    private float startZ; // 最初のZ座標

    void Start()
    {
        startZ = transform.position.z;
    }

    void Update()
    {
        // 現在位置が初期位置より後ろにいるときだけ移動
        if (transform.position.z < startZ)
        {
            Vector3 pos = transform.position;
            pos.z += moveSpeed * Time.deltaTime;
            transform.position = pos;
        }
    }
}
