using UnityEngine;

public class MoveForwardAndDestroy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float destroyZ = -20f;

    void Update()
    {
        // Z方向に移動（ワールド空間）
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);

        // 画面外に行ったら破棄
        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }
}
