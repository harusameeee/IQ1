using UnityEngine;

public class PlayerLineMove : MonoBehaviour
{
    public Transform[] lanes;
    public int currentLane = 0;
    public int maxLane = 2;

    public float jumpHeight = 2.0f;   // ジャンプの高さ
    public float jumpDuration = 0.6f; // 上昇＋下降にかかる時間
    public LayerMask groundLayer;
    public int playerNumber = 1;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isJumping = false;
    private float jumpTimer = 0f;
    private Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true; // 常にKinematicでもOK
    }

    void Update()
    {
        // Ground判定
        isGrounded = !isJumping && CheckIfGrounded();

        // 入力設定
        string joyLeft = playerNumber == 1 ? "joystick 1 button 5" : "joystick 2 button 5";
        string joyRight = playerNumber == 1 ? "joystick 1 button 4" : "joystick 2 button 4";
        string joyJump = playerNumber == 1 ? "joystick 1 button 0" : "joystick 2 button 0";

        KeyCode keyLeft = playerNumber == 1 ? KeyCode.D : KeyCode.RightArrow;
        KeyCode keyRight = playerNumber == 1 ? KeyCode.A : KeyCode.LeftArrow;
        KeyCode keyJump = playerNumber == 1 ? KeyCode.W : KeyCode.UpArrow;

        // 左右移動
        if ((Input.GetKeyDown(joyLeft) || Input.GetKeyDown(keyLeft)) && !isJumping)
        {
            currentLane = Mathf.Max(0, currentLane - 1);
            MoveToLane();
        }
        if ((Input.GetKeyDown(joyRight) || Input.GetKeyDown(keyRight)) && !isJumping)
        {
            currentLane = Mathf.Min(maxLane, currentLane + 1);
            MoveToLane();
        }

        // ジャンプ開始
        if ((Input.GetKeyDown(joyJump) || Input.GetKeyDown(keyJump)) && isGrounded && !isJumping)
        {
            Debug.Log($"P{playerNumber} ジャンプ開始！");
            isJumping = true;
            jumpTimer = 0f;
            startPos = transform.position;
        }

        // ジャンプ中の処理（KinematicでもOK）
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            float t = jumpTimer / jumpDuration;
            float height = Mathf.Sin(Mathf.PI * t) * jumpHeight; // 放物線的な動き
            transform.position = new Vector3(transform.position.x, startPos.y + height, transform.position.z);

            // 終了判定
            if (t >= 1f)
            {
                isJumping = false;
                transform.position = new Vector3(transform.position.x, startPos.y, transform.position.z);
            }
        }
    }

    void MoveToLane()
    {
        if (lanes != null && lanes.Length > currentLane)
        {
            Vector3 lanePos = lanes[currentLane].position;
            transform.position = new Vector3(lanePos.x, transform.position.y, lanePos.z);
        }
    }

    private bool CheckIfGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * 1.1f, Color.red);
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }
}
