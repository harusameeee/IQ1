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
    
    private float current_max_gcd = 0f;
    private float gcd_timer = 0f;
    public skilldata[] skills = new skilldata[3];
    public Skill_icon[] skill_icons = new Skill_icon[3];

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true; // 常にKinematicでもOK
    }

    void Update()
    {
        

        // 入力設定
        string joyLeft = playerNumber == 1 ? "joystick 1 button 5" : "joystick 2 button 5";
        string joyRight = playerNumber == 1 ? "joystick 1 button 4" : "joystick 2 button 4";
        string joyJump = playerNumber == 1 ? "joystick 1 button 0" : "joystick 2 button 0";

        string joyAttack = playerNumber == 1 ? "joystick 1 button 1" : "joystick 2 button 1";

        string joySkill = playerNumber == 1 ? "joystick 1 button 0" : "joystick 2 button 0";

        string joyDefense = playerNumber == 1 ? "joystick 1 button 0" : "joystick 2 button 0";
        for (int i = 0; i < skills.Length; i++)
        {
            if(skills[i].currentcooldown > 0)
            {

                skill_icons[i].gcd_icon.material.SetFloat("_removesegment", gcd_timer / current_max_gcd);
                if(skills[i].has_cooldown&&skills[i].currentcooldown > 0)
                {
                    skills[i].currentcooldown -= Time.deltaTime;
                    if (skills[i].maxstacks > skills[i].currentstacks&&skills[i].currentcooldown <= 0)
                    {
                        skills[i].currentstacks += 1;
                        skills[i].currentcooldown = skills[i].cooldown;
                        skill_icons[i].stacks_text.text = skills[i].currentstacks.ToString();
                    }
                }
            }
        }

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
        if ((Input.GetKeyDown(joyJump) || Input.GetKeyDown(keyJump)) && !isJumping)
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

    
}
