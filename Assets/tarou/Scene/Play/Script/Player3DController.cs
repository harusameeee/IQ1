using UnityEngine;
using System.Collections;

// Rigidbody必須
[RequireComponent(typeof(Rigidbody))]
public class Player3DController : MonoBehaviour
{
    public float moveSpeed = 5f;         // 通常時の移動速度
    public float slowMoveSpeed = 2f;     // スロウ時の移動速度
    public float jumpForce = 7f;         // ジャンプ力

    public LayerMask groundLayer;

    public int playerNumber = 1;         // プレイヤー番号（1か2）

    private Rigidbody rb;
    private bool isGrounded;

    // ステート管理（ノーマル or スロウ）
    public enum State { Normal, Slow }
    public State currentState = State.Normal;

    private Coroutine slowCoroutine;

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

        // ステートによって移動速度を切り替え
        float speed = currentState == State.Normal ? moveSpeed : slowMoveSpeed;
        rb.velocity = new Vector3(moveInput * speed, rb.velocity.y, 0f);
    }

    private bool CheckIfGrounded()
    {
        // 地面判定
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    // EnemyAttackタグのオブジェクトに当たったときにスロウに切り替え、1秒後ノーマルへ戻す
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            // 連続で当たった場合もコルーチンを正しく管理
            if (slowCoroutine != null) StopCoroutine(slowCoroutine);
            slowCoroutine = StartCoroutine(SwitchToSlowAndReturn());
        }
    }

    // スロウ状態に1秒したらノーマルに戻す
    private IEnumerator SwitchToSlowAndReturn()
    {
        currentState = State.Slow;
        yield return new WaitForSeconds(1f);
        currentState = State.Normal;
        slowCoroutine = null;
    }
}