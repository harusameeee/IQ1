using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.0f;         // �ʏ펞�̈ړ����x
    private float rotationSpeed = 5.0f;
    [SerializeField] public int playerNumber;        // �v���C���[�ԍ��i1��2�j

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // �v���C���[���]����Ȃ��悤��    
    }

    void Update()
    {
        // �v���C���[���ƂɈړ�Axis�𕪂���
        string horizontalAxis = playerNumber == 1 ? "Horizontal" : "Horizontal2";
        string verticalAxis = playerNumber == 1 ? "Vertical" : "Vertical2";

        // Raw���͂Ńs�^�b�Ǝ~�܂�
        float moveX = Input.GetAxisRaw(horizontalAxis);
        float moveZ = Input.GetAxisRaw(verticalAxis);

        // ���͕����Ɉړ�
        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized * moveSpeed;

        // ���͂�����Ƃ�����������ς���
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
