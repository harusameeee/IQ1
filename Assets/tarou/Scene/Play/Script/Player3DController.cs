using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player3DController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public LayerMask groundLayer;

    public int playerNumber = 1; // プレイヤー番号（1か2）

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        isGrounded = CheckIfGrounded();

        // プレイヤーごとにジャンプ入力を分ける
        string jumpButton = playerNumber == 1 ? "joystick 1 button 0" : "joystick 2 button 0";
        if (Input.GetKeyDown(jumpButton) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);
        }
    }

    void FixedUpdate()
    {
        // プレイヤーごとに移動Axisを分ける
        string horizontalAxis = playerNumber == 1 ? "Horizontal" : "Horizontal2";
        float moveInput = Input.GetAxis(horizontalAxis);
        rb.velocity = new Vector3(moveInput * moveSpeed, rb.velocity.y, 0f);
    }

    private bool CheckIfGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }
}