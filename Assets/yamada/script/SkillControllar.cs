
using UnityEngine;
using UnityEngine.VFX;
using static playerspawner;

// 同じGameObjectへの複数アタッチ阻止
[DisallowMultipleComponent]
public class SkillControllar : MonoBehaviour
{
    // スキルエフェクト群
    [Header("SkillEffects Settings")]
    // パーティクルエフェクト
    [SerializeField] private ParticleSystem[] particles;
    // VisualEffect
    [Tooltip("HierarchyにあるVisual Effectのオブジェクトを参照する(VFXGraphそのものではない)")]
    [SerializeField] private VisualEffect[] vEffects;
    // 盾オブジェクト
    [SerializeField] private GameObject shieldObj;

    // 職業
    private class_type jobType;

    // 入力情報
    string joyJump, joySkill1, joySkill2, joySkill3;
    KeyCode k;

    // 盾のオン/オフ
    bool isActive;

    void Awake()
    {
        jobType = GetJobType();
    }

    void Start()
    {
        joyJump = "joystick 1 button 0";
        joySkill1 = "joystick 1 button 1";
        joySkill2 = "joystick 1 button 2";
        joySkill3 = "joystick 1 button 3";

        isActive = false;
        k = KeyCode.Space;

        shieldObj.SetActive(false);

        vEffects[0].SendEvent("OnStop");
        vEffects[1].SendEvent("OnStop");
        vEffects[2].SendEvent("OnStop");
        vEffects[3].SendEvent("OnStop");
        vEffects[4].SendEvent("OnStop");

        particles[0].Stop();

        jobType = GetJobType();
    }

    void Update()
    {
        if (Input.GetKey(joySkill1))
        {
            PlaySkill1();
        }
        if(Input.GetKey(joySkill2))
        {
            PlaySkill2();
        }
        if (Input.GetKey(joySkill3))
        {
            PlaySkill3();
        }

        foreach (var vfx in vEffects)
        {
            Debug.Log($"{vfx.name} alive={vfx.aliveParticleCount}");
        }
    }

    // スキル1
    #region
    public void PlaySkill1()
    {
        switch (jobType)
        {
            // マーライオン
            #region
            case class_type.merlion:
                vEffects[0].SendEvent("OnPlay");
                break;
            #endregion

            // 忍者
            #region
            case class_type.ninja:
                shieldObj.SetActive(true);
                //vEffects[0].SendEvent("OnPlay");
                vEffects[1].SendEvent("OnPlay");
                vEffects[2].SendEvent("OnPlay");
                break;
            #endregion

            // トントゥ
            #region
            case class_type.tonto:
                break;
            #endregion
        }
    }
    #endregion

    // スキル2
    #region
    public void PlaySkill2()
    {
        switch (jobType)
        {
            // マーライオン
            #region
            case class_type.merlion:
                vEffects[1].SendEvent("OnPlay");
                vEffects[2].SendEvent("OnPlay");
                break;
            #endregion

            // 忍者
            #region
            case class_type.ninja:
                shieldObj.SetActive(true);
                vEffects[0].SendEvent("OnPlay");
                break;
            #endregion

            // トントゥ
            #region
            case class_type.tonto:
                break;
                #endregion
        }
    }
    #endregion

    // スキル3
    #region
    public void PlaySkill3()
    {
        switch (jobType)
        {
            // マーライオン
            #region
            case class_type.merlion:
                vEffects[3].SendEvent("OnPlay");
                vEffects[4].SendEvent("OnPlay");
                break;
            #endregion

            // 忍者
            #region
            case class_type.ninja:
                shieldObj.SetActive(true);
                vEffects[0].SendEvent("OnPlay");
                break;
            #endregion

            // トントゥ
            #region
            case class_type.tonto:
                break;
            #endregion
        }
    }
    #endregion


    // 職業参照
    class_type GetJobType()
    {
        string n = gameObject.name.ToLower();

        if (n.Contains("merlion"))
            return class_type.merlion;
        if (n.Contains("ninja"))
            return class_type.ninja;
        if (n.Contains("tonto"))
            return class_type.tonto;

        return class_type.merlion;
    }
}
