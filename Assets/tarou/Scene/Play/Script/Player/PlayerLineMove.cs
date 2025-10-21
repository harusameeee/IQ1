using System;
using System.Collections;
using System.Collections.Generic;
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
    [Header("combat system")]
    public float current_max_gcd = 0f;
    public bool has_coins = false;
    public int current_coins = 0;//max coins is inf
    public float gcd_timer = 0f;
    public int max_hp = 100;
    public List<skilldata> skills ;
    public List<Skill_icon> skill_icons;
    //hiddenvals
    string joyLeft,joyRight,joyJump,joyAttack,joyAttack2,joySkill,joyDefense;

    KeyCode keyLeft, keyRight, keyJump, keyAttack, keyAttack2, keySkill, keyDefense;
    
    
    [Header("test move")]

    [SerializeField] Vector3 velocity = Vector3.zero;
    
    [SerializeField] float speed = 10f;
    [SerializeField] bool isLine = false;
    public Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true; // 常にKinematicでもOK
        for(int i = 0; i < skills.Count; i++)
        {
            Debug.Log($"skill{i}:{skills[i].skillname}");
            skill_icons[i].setskill(skills[i], this);
        }
        
        joyLeft = playerNumber == 1 ? "joystick 1 button 5" : "joystick 2 button 5";
        joyRight = playerNumber == 1 ? "joystick 1 button 4" : "joystick 2 button 4";
        joyJump = playerNumber == 1 ? "joystick 1 button 0" : "joystick 2 button 0";
        joyAttack = playerNumber == 1 ? "joystick 1 button 1" : "joystick 2 button 1";
        joyAttack2 = playerNumber == 1 ? "joystick 1 button 2" : "joystick 2 button 2";
        joySkill = playerNumber == 1 ? "joystick 1 button 0" : "joystick 2 button 0";
        joyDefense = playerNumber == 1 ? "joystick 1 button 0" : "joystick 2 button 0";

        keyLeft = playerNumber == 1 ? KeyCode.RightArrow : KeyCode.D;
        keyRight = playerNumber == 1 ? KeyCode.LeftArrow : KeyCode.A;
        keyJump = playerNumber == 1 ? KeyCode.UpArrow : KeyCode.W;
        keyAttack = playerNumber == 1 ? KeyCode.Alpha1 : KeyCode.Z;
        keyAttack2 = playerNumber == 1 ? KeyCode.Alpha2 : KeyCode.X;
        keySkill = playerNumber == 1 ? KeyCode.Alpha3 : KeyCode.C;
        keyDefense = playerNumber == 1 ? KeyCode.Alpha4 : KeyCode.V;
    }

    void Update()
    {
        refresh_cds();
        useskills();
        movement();
        // 入力設定
        
    }

    void MoveToLane()
    {
        if (lanes != null && lanes.Length > currentLane)
        {
            Vector3 lanePos = lanes[currentLane].localPosition;
            StartCoroutine(lerplane());
        }
    }
    public void refresh_cds()
    {


        if (gcd_timer > 0)
        {
            gcd_timer -= Time.deltaTime;
        }        
        for (int i = 0; i < skills.Count; i++)
        {
            if(skills[i].currentcooldown > 0)
            {

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

    }
    void useskills()
    {
        if (Input.GetKeyDown(keySkill) || Input.GetKeyDown(joySkill))
        {
            if (evaluateskilluse(skills[2]))
            {
                gcd_timer = skills[2].gcd;
                current_max_gcd = skills[2].gcd;
                Debug.Log("use skill");
                animator.Play(skills[2].skillname);
            }
        }
        else if (Input.GetKeyDown(keyDefense) || Input.GetKeyDown(joyDefense))
        {
            if (evaluateskilluse(skills[3]))
            {
                Debug.Log("use def");
                gcd_timer = skills[3].gcd;
                current_max_gcd = skills[3].gcd;
                animator.Play(skills[3].skillname);
            }
        }
        else if (Input.GetKeyDown(keyAttack) || Input.GetKeyDown(joyAttack))
        {
            if (evaluateskilluse(skills[0]))
            {
                gcd_timer = skills[0].gcd;
                current_max_gcd = skills[0].gcd;
                Debug.Log("use attack");
                animator.Play(skills[0].skillname);
            }
        }
        else if (Input.GetKeyDown(keyAttack2) || Input.GetKeyDown(joyAttack2))
        {
            if (evaluateskilluse(skills[1]))
            {
                Debug.Log("use attack2");
                gcd_timer = skills[1].gcd;
                current_max_gcd = skills[1].gcd;
                animator.Play(skills[1].skillname);
            }
        }
    }
    bool evaluateskilluse(skilldata skill)
    {
        if(gcd_timer > 0)
        {
            Debug.Log("GCD active");
            return false;
        }
        if (skill.has_cooldown)
        {
            if (skill.currentstacks > 0)
            {
                skill.currentstacks -= 1;
                Debug.Log($"skill stacks left:{skill.currentstacks}");
                return true;
            }
            return false;
        }
        else if (skill.coincost > 0)
        {
            //コイン消費判定
            if (has_coins && current_coins >= skill.coincost)
            {
                current_coins -= skill.coincost;
                return true;
            }
            return false;
        }
        else
        {
            Debug.Log("no cost skill used");
            return true;
        }
    }
    void movement()
    {        // 左右移動
        
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, groundLayer);
        if (!isLine)
        {
            int moveInput = 0;
            if (Input.GetKey(joyLeft) || Input.GetKey(keyLeft))
            {
                moveInput = -1;
            }
            else if (Input.GetKey(joyRight) || Input.GetKey(keyRight))
            {
                moveInput = 1;
            }
            velocity = velocity * 0.8f + new Vector3(moveInput * speed, 0, 0);
            transform.localPosition += velocity * Time.deltaTime;
            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -7.5f, 7.5f), transform.localPosition.y, transform.localPosition.z);
            // 地面判定
        }
        else
        {
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
        }
        if ((Input.GetKeyDown(joyJump) || Input.GetKeyDown(keyJump)) && !isJumping)
        {
            Debug.Log($"P{playerNumber} ジャンプ開始！");
            isJumping = true;
            jumpTimer = 0f;
            startPos = transform.localPosition;
        }
        // ジャンプ中の処理（KinematicでもOK）
        if (isJumping)
        {
            if (isLine)
            {
                    
                jumpTimer += Time.deltaTime;
                float t = jumpTimer / jumpDuration;
                if (t >= 1f)
                {
                    isJumping = false;
                    
                }
            }
            else
            {
            jumpTimer += Time.deltaTime;
            float t = jumpTimer / jumpDuration;
            float height = Mathf.Sin(Mathf.PI * t) * jumpHeight; // 放物線的な動き
            transform.localPosition = new Vector3(transform.localPosition.x, startPos.y + height, transform.localPosition.z);

            // 終了判定
            if (t >= 1f)
            {
                isJumping = false;
                transform.localPosition = new Vector3(transform.localPosition.x, startPos.y, transform.localPosition.z);
            }
            }

        }
    }
    public IEnumerator lerplane()
    {
        float duration = 0.1f; // 補間にかける時間
        float elapsed = 0f;
        Vector3 initialPos = transform.localPosition;
        Vector3 targetPos = new Vector3(lanes[currentLane].localPosition.x, transform.localPosition.y,  transform.localPosition.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localPosition = Vector3.Lerp(initialPos, targetPos, t);
            yield return null;
        }
        transform.localPosition = targetPos; // 最終的にターゲット位置にセット
    }

    
}
