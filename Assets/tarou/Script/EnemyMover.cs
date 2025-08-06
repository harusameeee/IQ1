using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public float moveDistance = 5f;     // Z方向にどれだけ動かすか
    public float moveSpeed = 3f;        // 移動速度
    private bool shouldMove = false;
    private Vector3 targetPosition;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Playerと接触！");
            shouldMove = true;
            targetPosition = transform.position + Vector3.forward * moveDistance;
        }
    }

    void Update()
    {
        if (shouldMove)
        {
            // Z方向に徐々に移動
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 移動終了したら止める
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                shouldMove = false;
            }
        }
    }
}
