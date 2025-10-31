using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2.0f;         // 動く速さ
    private float rotationSpeed = 5.0f;
    [SerializeField] public int playerNumber;        // プレイヤー識別番号

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // 回転を止める    
    }

    void Update()
    {
        // プレイヤーを識別
        string horizontalAxis = playerNumber == 1 ? "Horizontal" : "Horizontal2";
        string verticalAxis = playerNumber == 1 ? "Vertical" : "Vertical2";

        // 移動方向を取得
        float moveX = Input.GetAxisRaw(horizontalAxis);
        float moveZ = Input.GetAxisRaw(verticalAxis);

        // 移動
        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized * moveSpeed;

        // 一定の入力値を超えたら
        if (move.magnitude > 0.1f)
        {
            rb.linearVelocity = new Vector3(move.x, 0.0f, move.z);

            Quaternion targetRotation = Quaternion.LookRotation(-move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }
}
