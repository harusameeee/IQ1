using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// アニメーターのattack_upもしくはattack_sideがtrueなら武器を右手の位置に出現
// 座標は右手目安で左手との差分で回転を入れる
// ボーンが必要？

public class AppWeapon : MonoBehaviour
{
    // 魔女のアニメーター
    [SerializeField] Animator anim = null;
    // 武器モデル
    [SerializeField] GameObject Weapon = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 武器を出す
    public void HaveAWeapon()
    {
        anim.SetBool("attack_up",true);
    }
}
