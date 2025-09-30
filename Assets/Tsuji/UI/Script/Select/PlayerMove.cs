using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.0f;         // 通常時の移動速度
    private float rotationSpeed = 5.0f;
    [SerializeField] public int playerNumber;        // プレイヤー番号（1か2）

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // プレイヤーが転がらないように    
    }

    void Update()
    {
        // プレイヤーごとに移動Axisを分ける
        string horizontalAxis = playerNumber == 1 ? "Horizontal" : "Horizontal2";
        string verticalAxis = playerNumber == 1 ? "Vertical" : "Vertical2";

        // Raw入力でピタッと止まる
        float moveX = Input.GetAxisRaw(horizontalAxis);
        float moveZ = Input.GetAxisRaw(verticalAxis);

        // 入力方向に移動
        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized * moveSpeed;

        // 入力があるときだけ向きを変える
        if (move.magnitude > 0.1f)
        {
            rb.velocity = new Vector3(move.x, 0.0f, move.z);

            Quaternion targetRotation = Quaternion.LookRotation(-move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
    }
}
