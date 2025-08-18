using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player3DController1 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Z軸の移動を固定
        //rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    void Update()
    {
        // 地面との接触をレイヤーで判定
        isGrounded = CheckIfGrounded();

        // ジャンプ処理（Wキーでジャンプ）
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);
        }
    }

    void FixedUpdate()
    {
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.A)) moveInput = -1f;
        if (Input.GetKey(KeyCode.D)) moveInput = 1f;

        rb.velocity = new Vector3(moveInput * moveSpeed, rb.velocity.y, 0f);
    }

    // 地面との接触判定：レイヤーでのみ判定
    private bool CheckIfGrounded()
    {
        // プレイヤーの足元の少し下にレイを飛ばす
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }
}
