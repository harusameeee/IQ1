using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLineMove : entity
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
    [HideInInspector]public float current_hp = 100;
    public float max_hp = 100;
    public List<skilldata> skills ;
    public hitbox hb;    public override Vector2 position => new Vector2(-transform.localPosition.x, transform.localPosition.y + 1.5f);
    public Vector2 dim;
    public override Vector2 dimension => dim;
    public player_ui ui;
    //hiddenvals
    string joyLeft,joyRight,joyJump,joyAttack,joyAttack2,joySkill,joyDefense;

    KeyCode keyLeft, keyRight, keyJump, keyAttack, keyAttack2, keySkill, keyDefense;
    
    
    [Header("test move")]

    [SerializeField] Vector3 velocity = Vector3.zero;
    
    [SerializeField] float speed = 10f;
    [SerializeField] bool isLine = false;
    public Animator animator;
    public override void Start()
    {
        hb.owner = this;
        
        ui.hp_bar.value = (float)current_hp / max_hp;
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true; // 常にKinematicでもOK
        for(int i = 0; i < skills.Count; i++)
        {
            skills[i].currentcooldown = 0;
        
            skills[i].currentstacks = skills[i].maxstacks;
            Debug.Log($"skill{i}:{skills[i].skillname}");
            ui.skill_icons[i].setskill(skills[i], this);
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
        if (has_coins)
        {
            ui.coin_texttransform.gameObject.SetActive(true);
        }
        else
        {
            ui.coin_texttransform.gameObject.SetActive(false);
        }
    }

    public override void Update()
    {
        base.Update();//countdown buff durations
        refresh_cds();//refresh cooldowns
        useskills();//for activating skills
        movement();// for movement
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


        float skill_cd_speed = 1.0f;
        float gcd_speed = 1.0f; 
        foreach (var buff in buffs)
        {
            if (buff.type == bufftypes.cooldown_reduction)
            {
                skill_cd_speed += buff.pow;
            }
            else if (buff.type == bufftypes.gcd_reduction)
            {
                gcd_speed += buff.pow;
            }
        }
        if (gcd_timer > 0)
        {
            gcd_timer -= Time.deltaTime*gcd_speed;
        }        
        for (int i = 0; i < skills.Count; i++)
        {
            if(!skills[i].has_cooldown)
            {
                continue;
            }
            if (skills[i].currentcooldown > 0 )
            {
                if (skills[i].maxstacks > skills[i].currentstacks)
                {
                    skills[i].currentcooldown -= Time.deltaTime*skill_cd_speed;
                }

            }
            if (skills[i].currentcooldown <= 0)
            {
                //Debug.Log($"skill{i} cooldown finished");
                skills[i].currentstacks += 1;
                skills[i].currentcooldown = skills[i].cooldown;
                ui.skill_icons[i].stacks_text.text = skills[i].currentstacks.ToString();
            }
        }

    }
    void useskills()
    {
        int atkval = -1;
        if (Input.GetKey(keySkill) || Input.GetKey(joySkill))
        {
            atkval = 2;
        }
        else if (Input.GetKey(keyDefense) || Input.GetKey(joyDefense))
        {
            atkval = 3;
        }
        else if (Input.GetKey(keyAttack) || Input.GetKey(joyAttack))
        {
            atkval = 0;
        }
        else if (Input.GetKey(keyAttack2) || Input.GetKey(joyAttack2))
        {
            atkval = 1;
        }
        if (atkval != -1)
        {
            if (evaluateskilluse(skills[atkval]))
            {
                foreach (var effect in skills[atkval].onUse_effects)
                {
                    effect.activeeffect(this, this);
                }
                hb.skilldata = skills[atkval];
                gcd_timer = skills[atkval].gcd;
                current_max_gcd = skills[atkval].gcd;
                current_coins -= skills[atkval].coincost;
                ui.coin_text.text = current_coins.ToString();
                if (skills[atkval].has_cooldown && skills[atkval].currentstacks > 0)
                {
                    skills[atkval].currentstacks -= 1;
                }
                Debug.Log($"skill stacks left:{skills[atkval].currentstacks}");
                animator.Play(skills[atkval].skillname);
            }
        }
    }
    public void resetskillcd(int skillindex)
    {
        if(skillindex>=0&& skillindex<skills.Count)
        {
            skills[skillindex].currentcooldown = 12;
            skills[skillindex].currentstacks = skills[skillindex].maxstacks;

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
            if (skill.currentstacks <= 0)
            {
                Debug.Log("no skill stacks left");
                return false;
            }

        }
        if (skill.coincost > 0)
        {
            //コイン消費判定
            if (current_coins < skill.coincost)
            {
                
                Debug.Log("not enough coins");
                return false;
            }
        }        
        return true;
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
            float speedbuff = 1;
            foreach (var buff in buffs)
            {
                if (buff.type == bufftypes.speed_increase)
                {
                    speedbuff += buff.pow;
                }
            }
            velocity = velocity * 0.8f + new Vector3(moveInput * speed * speedbuff, 0, 0);
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
        Vector3 targetPos = new Vector3(lanes[currentLane].localPosition.x, transform.localPosition.y, transform.localPosition.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localPosition = Vector3.Lerp(initialPos, targetPos, t);
            yield return null;
        }
        transform.localPosition = targetPos; // 最終的にターゲット位置にセット
    }
    public override bool TakeDamage(float damageAmount,bool comboable = true)
    {
        if(buffs.Exists(buff => buff.type == bufftypes.invuln|| buff.type == bufftypes.stealth))
        {
            Debug.Log($"P{playerNumber} is invulnerable and took no damage.");
            return false;
        }
        current_hp -= damageAmount;
        ui.hp_bar.value = (float)current_hp / max_hp;
        Debug.Log($"P{playerNumber} took {damageAmount} damage. Current HP: {current_hp}");
        if (current_hp <= 0)
        {
            Debug.Log($"P{playerNumber} is defeated!");
        }
        
        onHit?.Invoke(-damageAmount,false);
        return true;
    }




}
