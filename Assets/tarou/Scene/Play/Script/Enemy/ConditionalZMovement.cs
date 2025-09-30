using UnityEngine;
using System.Collections;

public class ConditionalZMovement : MonoBehaviour
{
    // Z方向への移動速度
    public float moveSpeed = 1f;
    // 攻撃後に追加で移動するZ座標の量
    public float addZ = 10f;
    // 追加移動にかける時間
    public float addZDuration = 2f;

    // 初期のZ座標を記録
    private float startZ;
    // 攻撃中など待機状態かどうか
    private bool isWaiting = false;
    // 警告UI（DangerUI）
    public GameObject Danger;             

    // 状態
    public enum State { Start, Fly, Attack1, Attack2, None }
    public State currentState = State.None;

    // アニメーターへの参照
    private Animator animator;            

    void Start()
    {
        // 初期位置のZ座標を保存
        startZ = transform.position.z;

        animator = GetComponent<Animator>();

        // DangerUIをタグから探す
        Danger = GameObject.FindGameObjectWithTag("DangerUI");

        if (Danger != null)
        {
            // 最初は非表示にしておく
            Danger.SetActive(false);

            // 状態をStartに設定
            SetState(State.Start);
        }
    }

    void Update()
    {
        // 攻撃中など待機状態なら動作しない
        if (isWaiting) return;

        // 初期位置より手前にいたら、Z方向に前進させる
        if (transform.position.z < startZ)
        {
            Vector3 pos = transform.position;
            pos.z += moveSpeed * Time.deltaTime;
            transform.position = pos;

            // 飛行中の状態にする
            if (currentState != State.Fly)
            {
                SetState(State.Fly);
            }
        }

        // Z座標が小さくなったら攻撃を開始
        if (transform.position.z < -115f && !isWaiting)
        {
            StartCoroutine(AttackAndAddZ());
        }
    }

    private IEnumerator AttackAndAddZ()
    {
        isWaiting = true; // 待機状態に入る

        // ランダムでAttack1かAttack2を選ぶ
        State attackState = (Random.value < 0.5f) ? State.Attack1 : State.Attack2;

        // DangerUIを表示
        if (Danger != null) Danger.SetActive(true);

        // 状態を攻撃に設定
        SetState(attackState);

        // 5秒間攻撃状態を維持
        yield return new WaitForSeconds(5f);

        // 攻撃後にZ方向へゆっくり移動する
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0f, 0f, addZ);

        // 指定時間かけて線形補間で移動
        while (elapsed < addZDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / addZDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos; 

        // DangerUIを非表示にする
        if (Danger != null) Danger.SetActive(false);

        // 待機解除
        isWaiting = false;

        // 状態をStartに戻す
        SetState(State.Start);
    }

    private void SetState(State state)
    {
        // 現在の状態を更新
        currentState = state;

        // Animatorが存在すれば整数値でパラメータを渡す
        if (animator != null)
        {
            animator.SetInteger("state", (int)state);
        }
    }
}
