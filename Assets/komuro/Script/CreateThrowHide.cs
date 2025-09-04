using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateThrowHide : MonoBehaviour
{
    // 攻撃をする間隔
    [SerializeField] float attackTime = 3.0f;
    // 攻撃してからの経過時間
    private float AttackInterval = 0.0f;
    // 攻撃できるか
    private bool isAttack;
    // CubeプレハブをGameObject型で取得
    [SerializeField]GameObject obj;


    // Start is called before the first frame update
    void Start()
    {
        isAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttack)
        {
            // Cubeプレハブを元に、インスタンスを生成、
            Instantiate(obj, new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
            // isAttackをfalseにする
            isAttack = false;
        }
        else
        {
            // 攻撃間隔
            AttackInterval += Time.deltaTime;
            if(attackTime < AttackInterval)
            {
                isAttack = true;
                AttackInterval = 0.0f;
            }
        }
    }
}
