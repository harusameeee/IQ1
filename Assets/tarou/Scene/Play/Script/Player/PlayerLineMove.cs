using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLineMove : entity
{
    public Transform[] lanes = new Transform[5];
    public int currentLane = 0;
    public int maxLane = 2;

    public float jumpHeight = 2.0f;   // �����פι⤵
    public float jumpDuration = 0.6f; // �徺�ܲ��ߤˤ�����E���
    public LayerMask groundLayer;
    public LayerMask levelLayer;
    public int playerNumber = 1;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isJumping = false;
    private float jumpTimer = 0f;
    public Sprite profilepic;
    private Vector3 startPos;
    [Header("combat system")]
    public float current_max_gcd = 0f;
    public bool has_coins = false;
    public int current_coins = 0;//max coins is inf
    public float gcd_timer = 0f;
    [HideInInspector] public float current_hp = 100;
    public float max_hp = 100;
    public List<skilldata> skills;
    public hitbox hb; public override Vector2 position => new Vector2(-transform.localPosition.x, transform.localPosition.y + 1.5f);
    public Vector2 dim;
    public override Vector2 dimension => dim;
    public player_ui ui;
    //hiddenvals
    string joyLeft, joyRight, joyJump, joyAttack, joyAttack2, joySkill, joyDefense, joyDefense2;

    KeyCode keyLeft, keyRight, keyJump, keyAttack, keyAttack2, keySkill, keyDefense;


    [Header("test move")]

    [SerializeField] Vector3 velocity = Vector3.zero;

    [SerializeField] float speed = 10f;
    [SerializeField] bool isLine = false;
    public Animator animator;
    [HideInInspector] public PlayerLineMove otherplayer;
    public player_canvas_handler playercanvas;
    float moveInput = 0f;
    public override void Start()
    {

        hb.owner = this;

        bufficonparent = ui.skill_icon_transform;
        showbufficons = true;
        ui.hp_bar.value = (float)current_hp / max_hp;
        base.Start();
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true; // ��E�Kinematic�Ǥ�OK
        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].currentcooldown = skills[i].cooldown;

            skills[i].currentstacks = skills[i].maxstacks;
            Debug.Log($"skill{i}:{skills[i].skillname}");
            ui.skill_icons[i].setskill(skills[i], this);
        }
        ui.profilepic_img.sprite = profilepic;
        if (!has_coins)
        {
            ui.coin_texttransform.gameObject.SetActive(false);
        }
        //joyLeft = playerNumber == 1 ? "joystick 1 button 5" : "joystick 2 button 5";
        //joyRight = playerNumber == 1 ? "joystick 1 button 4" : "joystick 2 button 4";
        joyJump = playerNumber == 1 ? "joystick 1 button 0" : "joystick 2 button 0";
        joyAttack = playerNumber == 1 ? "joystick 1 button 1" : "joystick 2 button 1";
        joyAttack2 = playerNumber == 1 ? "joystick 1 button 2" : "joystick 2 button 2";
        joySkill = playerNumber == 1 ? "joystick 1 button 3" : "joystick 2 button 3";
        joyDefense = playerNumber == 1 ? "joystick 1 button 5" : "joystick 2 button 5";
        joyDefense2 = playerNumber == 1 ? "joystick 1 button 4" : "joystick 2 button 4";

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
        playercanvas.owner = this;
    }

    public override void Update()
    {
        base.Update();//countdown buff durations
        refresh_cds();//refresh cooldowns

        if (current_hp > 0)
        {
            useskills();
        }
        //for activating skills
        movement();// for movement


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
            gcd_timer -= Time.deltaTime * gcd_speed;
        }
        for (int i = 0; i < skills.Count; i++)
        {
            if (!skills[i].has_cooldown)
            {
                continue;
            }
            if (skills[i].currentcooldown > 0)
            {
                if (skills[i].maxstacks > skills[i].currentstacks)
                {
                    skills[i].currentcooldown -= Time.deltaTime * skill_cd_speed;
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
        else if (Input.GetKey(keyDefense) || Input.GetKey(joyDefense2) || Input.GetKey(joyDefense))
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
                hb.effects = skills[atkval].onHit_effect;
                gcd_timer = skills[atkval].gcd;
                current_max_gcd = skills[atkval].gcd;
                // コイン消費
                current_coins -= skills[atkval].coincost;

                // コイン増加（coingainが正なら追加）
                if (skills[atkval].coingain > 0)
                {
                    AddCoins(skills[atkval].coingain);
                }
                else
                {
                    // 上の AddCoins が UI 更新するので、こちらは必要な時だけ表示更新
                    ui.coin_text.text = current_coins.ToString();
                }
                if (skills[atkval].has_cooldown && skills[atkval].currentstacks > 0)
                {
                    skills[atkval].currentstacks -= 1;
                }

                Debug.Log($"Trying to play: {skills[atkval].skillname}");
                animator.Play(skills[atkval].skillname);
            }
        }
    }
    public void resetskillcd(int skillindex)
    {
        if (skillindex >= 0 && skillindex < skills.Count)
        {
            skills[skillindex].currentcooldown = 12;
            skills[skillindex].currentstacks = skills[skillindex].maxstacks;

        }
    }
    bool evaluateskilluse(skilldata skill)
    {
        if (gcd_timer > 0)
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
            //���������ȽāE
            if (current_coins < skill.coincost)
            {

                Debug.Log("not enough coins");
                return false;
            }
        }
        return true;
    }
    void movement()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, groundLayer);
        if (!isLine)
        {
            moveInput = 0f;

            // �X�e�B�b�N���́i-1 �` +1�j
            float stickInput = playerNumber == 1 ? Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal2");

            // �{�^�����͂����p�i�������͑Ή��j
            //if (Input.GetKey(joyLeft) || Input.GetKey(keyLeft))
            //{
            //    moveInput = -1;
            //}
            //else if (Input.GetKey(joyRight) || Input.GetKey(keyRight))
            //{
            //    moveInput = 1;
            //}
            if (Mathf.Abs(stickInput) > 0.2f) // �X�e�B�b�N���͂����ȏ�Ȃ�̗p
            {
                moveInput = -stickInput;
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

            // X�ʒu����
            transform.localPosition = new Vector3(
                Mathf.Clamp(transform.localPosition.x, -8f, 8f),
                transform.localPosition.y,
                transform.localPosition.z
            );
        }
        else
        {
            // ���[���ړ����[�h
            //if ((Input.GetKeyDown(joyLeft) || Input.GetKeyDown(keyLeft)) && !isJumping)
            //{
            //    currentLane = Mathf.Max(0, currentLane - 1);
            //    MoveToLane();
            //}
            //if ((Input.GetKeyDown(joyRight) || Input.GetKeyDown(keyRight)) && !isJumping)
            //{
            //    currentLane = Mathf.Min(maxLane, currentLane + 1);
            //    MoveToLane();
            //}
        }

        // �W�����v�����i���̂܂܁j
        if ((Input.GetKeyDown(joyJump) || Input.GetKeyDown(keyJump)) && !isJumping && !buffs.Any(b => b.type == bufftypes.nojump))
        {
            Debug.Log($"P{playerNumber} �W�����v");
            isJumping = true;
            jumpTimer = 0f;
            startPos = transform.localPosition;
        }

        if (isJumping)
        {
            if (isLine)
            {
                jumpTimer += Time.deltaTime;
                float t = jumpTimer / jumpDuration;
                if (t >= 1f) isJumping = false;
            }
            else
            {
                jumpTimer += Time.deltaTime;
                float t = jumpTimer / jumpDuration;
                float height = Mathf.Sin(Mathf.PI * t) * jumpHeight;
                transform.localPosition = new Vector3(transform.localPosition.x, startPos.y + height, transform.localPosition.z);

                if (t >= 1f)
                {
                    isJumping = false;
                    transform.localPosition = new Vector3(transform.localPosition.x, startPos.y, transform.localPosition.z);
                }
            }
        }
        else
        {
            //Debug.Log("Raycasting to adjust Y position");
            //if(Physics.Raycast(transform.position+ Vector3.up*2f, Vector3.down, out RaycastHit hitInfo, 10f, levelLayer))
            //{
            //   Debug.Log("Hit level layer, adjusting Y position");
            //   transform.position = new Vector3(transform.position.x,hitInfo.point.y, transform.position.z); 
            //}

        }
    }

    public IEnumerator lerplane()
    {
        float duration = 0.1f; // ��֤ˤ�����E���
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
        transform.localPosition = targetPos; // �ǽ�Ū�˥������åȰ��֤˥��å�
    }
    public override bool TakeDamage(float damageAmount, bool comboable = true, List<damagable_type> damagable_Types = null, Vector2 hitpoint = new Vector2())
    {

        if (buffs.Exists(buff => buff.type == bufftypes.invuln || buff.type == bufftypes.stealth) || current_hp <= 0)
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
            StartCoroutine(becomeghost(10.0f));
        }
        else
        {
            StartCoroutine(dmgflash());
        }

        onHit?.Invoke(-damageAmount, false);
        return true;
    }
    public void heal(float healamount)
    {
        current_hp = Mathf.Min(current_hp + healamount, max_hp);
    }
    public override void addbuff(buffdata newBuff)
    {
        var existingBuff = buffs.Find(x => x.buffname == newBuff.buffname);
        if (existingBuff != null)
        {
            if (!newBuff.stackable) return;
            existingBuff.pow += newBuff.pow;
            existingBuff.duration = Mathf.Max(existingBuff.duration, newBuff.duration);
        }
        else
        {
            buffdata buffToAdd = newBuff.copy();
            buffs.Add(buffToAdd);
            if (showbufficons && buffToAdd.showbufficon)
            {
                var icon = bufficons.Find(b => !b.gameObject.activeSelf);
                if (icon != null)
                {

                    icon.gameObject.SetActive(true);
                    icon.referencedbuff = buffToAdd;
                    icon.buffimg.sprite = buffToAdd.icon;
                    icon.transform.SetAsLastSibling();
                }
            }
            if (buffToAdd.type == bufftypes.sticktogether || buffToAdd.type == bufftypes.stayaway ||
                buffToAdd.type == bufftypes.keep_moving || buffToAdd.type == bufftypes.Stop_moving)
            {
                playercanvas.addbuffvisual(ref buffToAdd);
            }

        }

    }

    public void exit_stayaway_buff(float pow)
    {
        if (Mathf.Abs(otherplayer.transform.localPosition.x - this.transform.localPosition.x) < 2)
        {
            Debug.Log("stay away buff exited, dealing damage");
            otherplayer.TakeDamage(pow, false, new List<damagable_type>(), new Vector2(otherplayer.transform.localPosition.x, otherplayer.transform.localPosition.y));
        }

    }
    public void exit_staytogether_buff(float pow)
    {
        if (Mathf.Abs(otherplayer.transform.localPosition.x - this.transform.localPosition.x) > 2)
        {
            Debug.Log("stay together buff exited, dealing damage");
            otherplayer.TakeDamage(pow, false, new List<damagable_type>(), new Vector2(otherplayer.transform.localPosition.x, otherplayer.transform.localPosition.y));
        }

    }
    public void exit_keepmoving_buff(float pow)
    {
        if (moveInput == 0)
        {
            Debug.Log("keep moving buff exited, dealing damage");
            TakeDamage(pow, false, new List<damagable_type>(), new Vector2(transform.localPosition.x, transform.localPosition.y));
        }
    }
    public void exit_stopmoving_buff(float pow)
    {
        if (moveInput != 0)
        {
            Debug.Log("stop moving buff exited, dealing damage");
            TakeDamage(pow, false, new List<damagable_type>(), new Vector2(transform.localPosition.x, transform.localPosition.y));
        }
    }
    public IEnumerator becomeghost(float duration)
    {
        //_Tweak_transparency
        Debug.Log("becoming ghost for " + duration + " seconds");
        // �ŏ��̓�����
        foreach (var mat in rend.materials)
        {
            mat.SetFloat("_Tweak_transparency", -0.9f);
        }

        yield return new WaitForSeconds(duration);

        // ���ɖ߂�

        foreach (var mat in rend.materials)
        {
            mat.SetFloat("_Tweak_transparency", 0);
        }

        current_hp = max_hp;
    }
    public IEnumerator hitstop(float duration)
    {
        animator.speed = 0;
        yield return new WaitForSecondsRealtime(duration);
        animator.speed = 1;
    }

    public override void PlayBuffVFX(bufftypes type)
    {
        //base.PlayBuffVFX(type); // ���ʂ̏������Ăԁi�C�Ӂj

        switch (type)
        {
            case bufftypes.speed_increase:
                var fx1 = Instantiate(Resources.Load<GameObject>("VFX/SpeedBuff_Player"), transform.position, Quaternion.identity);
                fx1.transform.SetParent(transform);
                break;

            case bufftypes.attack:
                var fx2 = Instantiate(Resources.Load<GameObject>("VFX/AttackBuff_Player"), transform.position, Quaternion.identity);
                fx2.transform.SetParent(transform);
                break;
        }
    }

    public void AddCoins(int amount)
    {
        current_coins += amount;
        ui.coin_text.text = current_coins.ToString();
    }
}
