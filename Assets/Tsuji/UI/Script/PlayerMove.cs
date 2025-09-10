using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float speed = 3.0f;

    Rigidbody rb;               //Rigidbody型の変数
    public float jumpPower=3.0f;     //ジャンプ力　アクセス修飾子をpublicに指定

    void Start()
    {
        rb = GetComponent<Rigidbody>();  //Rigidbodyを取得、変数に代入
    }
    void Update()
    {
        // Wキー（前方移動）
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += speed * transform.forward * Time.deltaTime;
        }

        // Sキー（後方移動）
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= speed * transform.forward * Time.deltaTime;
        }

        // Dキー（右移動）
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += speed * transform.right * Time.deltaTime;
        }

        // Aキー（左移動）
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= speed * transform.right * Time.deltaTime;
        }

        //上矢印キーが押されたとき
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Rigidbodyに上方向にJumpPowerの力を加える
            rb.AddForce(transform.up * jumpPower);
        }
    }
}