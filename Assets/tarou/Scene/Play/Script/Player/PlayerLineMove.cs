using UnityEngine;

public class PlayerLineMove : MonoBehaviour
{
    public Transform[] lanes;
    public int currentLane = 0;
    public int maxLane = 2;

    public float jumpForce = 5.0f;
    public LayerMask groundLayer;
    public int playerNumber = 1;

    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        isGrounded = CheckIfGrounded();

        // レーン移動ボタン
        string leftKey = playerNumber == 1 ? "joystick 1 button 5" : "joystick 2 button 5";
        string rightKey = playerNumber == 1 ? "joystick 1 button 4" : "joystick 2 button 4";

        if (Input.GetKeyDown(leftKey))
        {
            currentLane = Mathf.Max(0, currentLane - 1);
            MoveToLane();
        }
        if (Input.GetKeyDown(rightKey))
        {
            currentLane = Mathf.Min(maxLane, currentLane + 1);
            MoveToLane();
        }

        // ジャンプボタン
        string jumpButton = playerNumber == 1 ? "joystick 1 button 0" : "joystick 2 button 0";
        if (Input.GetKeyDown(jumpButton) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    void MoveToLane()
    {
        if (lanes != null && lanes.Length > currentLane)
        {
            Vector3 lanePos = lanes[currentLane].position;
            // Y座標は現在位置を維持（ジャンプ中も上書きしない！）
            transform.position = new Vector3(lanePos.x, transform.position.y, lanePos.z);
            // 横移動の慣性を消す（Yはジャンプのためそのまま）
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    private bool CheckIfGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }
}