using UnityEngine;
using System.Collections;

public class ConditionalZMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float addZ = 10f;
    public float addZDuration = 2f;

    private float startZ;
    private bool isWaiting = false;
    public GameObject Danger;

    public enum State { Start, Fly, Attack1, Attack2, None }
    public State currentState = State.None;

    private Animator animator;

    void Start()
    {
        startZ = transform.position.z;
        animator = GetComponent<Animator>();

        Danger = GameObject.FindGameObjectWithTag("DangerUI");

        if (Danger != null)
        {
            Danger.SetActive(false);
            SetState(State.Start);
        }
        else
        {
            Debug.LogWarning("DangerUIタグを持つオブジェクトが見つかりません");
        }
    }

    void Update()
    {
        if (isWaiting) return;

        if (transform.position.z < startZ)
        {
            Vector3 pos = transform.position;
            pos.z += moveSpeed * Time.deltaTime;
            transform.position = pos;

            // 飛行中としてみなす
            if (currentState != State.Fly)
            {
                SetState(State.Fly);
            }
        }

        if (transform.position.z < -60f && !isWaiting)
        {
            StartCoroutine(AttackAndAddZ());
        }
    }

    private IEnumerator AttackAndAddZ()
    {
        isWaiting = true;

        // ランダムでAttack1かAttack2を選択
        State attackState = (Random.value < 0.5f) ? State.Attack1 : State.Attack2;

        if (Danger != null) Danger.SetActive(true);

        SetState(attackState);

        yield return new WaitForSeconds(5f);

        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0f, 0f, addZ);

        while (elapsed < addZDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / addZDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;

        if (Danger != null) Danger.SetActive(false);

        isWaiting = false;
        SetState(State.Start);
    }

    private void SetState(State state)
    {
        currentState = state;
        if (animator != null)
        {
            animator.SetInteger("state", (int)state);
        }
    }
}