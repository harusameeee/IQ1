using UnityEngine;
using System.Collections;

// Rigidbody�K�{
[RequireComponent(typeof(Rigidbody))]
public class Player3DController : MonoBehaviour
{
    public float moveSpeed = 5f;         // �ʏ펞�̈ړ����x
    public float slowMoveSpeed = 2f;     // �X���E���̈ړ����x
    public float jumpForce = 7f;         // �W�����v��

    public LayerMask groundLayer;

    public int playerNumber = 1;         // �v���C���[�ԍ��i1��2�j

    private Rigidbody rb;
    private bool isGrounded;

    // �X�e�[�g�Ǘ��i�m�[�}�� , �X���E�j
    public enum State { Normal, Slow }
    public State currentState = State.Normal;

    private Coroutine slowCoroutine;
    private Coroutine blinkCoroutine;

    // �_�łŎg��
    private Renderer[] renderers;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // �S�Ă�Renderer�擾�i�q�I�u�W�F�N�g���܂ށj
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        isGrounded = CheckIfGrounded();

        // �v���C���[���ƂɃW�����v���͂𕪂���
        string jumpButton = playerNumber == 1 ? "joystick 1 button 0" : "joystick 2 button 0";
        if (Input.GetKeyDown(jumpButton) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, 0f);
        }
    }

    void FixedUpdate()
    {
        // �v���C���[���ƂɈړ�Axis�𕪂���
        string horizontalAxis = playerNumber == 1 ? "Horizontal" : "Horizontal2";
        float moveInput = Input.GetAxis(horizontalAxis);

        // �X�e�[�g�ɂ���Ĉړ����x��؂�ւ�
        float speed = currentState == State.Normal ? moveSpeed : slowMoveSpeed;
        rb.linearVelocity = new Vector3(moveInput * speed, rb.linearVelocity.y, 0f);
    }

    private bool CheckIfGrounded()
    {
        // �n�ʔ���
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    // EnemyAttack�^�O�̃I�u�W�F�N�g�ɓ��������Ƃ��ɃX���E�ɐ؂�ւ��A1�b��m�[�}���֖߂�
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            // �A���œ��������ꍇ���R���[�`���𐳂����Ǘ�
            if (slowCoroutine != null) StopCoroutine(slowCoroutine);
            slowCoroutine = StartCoroutine(SwitchToSlowAndReturn());

            // �_�ŃR���[�`�����Ǘ�
            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
            blinkCoroutine = StartCoroutine(Blink(1f, 0.1f));
        }

        if(other.CompareTag("magic camera"))
        {
            // �A���œ��������ꍇ���R���[�`���𐳂����Ǘ�
            if (slowCoroutine != null) StopCoroutine(slowCoroutine);
            slowCoroutine = StartCoroutine(SwitchToSlowAndReturn());
        }
    }

    // �X���E��Ԃ�1�b������m�[�}���ɖ߂�
    private IEnumerator SwitchToSlowAndReturn()
    {
        currentState = State.Slow;
        yield return new WaitForSeconds(1f);
        currentState = State.Normal;
        slowCoroutine = null;
    }

    // �_�ŃR���[�`��
    private IEnumerator Blink(float duration, float interval)
    {
        float elapsed = 0f;
        bool visible = true;
        while (elapsed < duration)
        {
            SetRenderersVisible(visible);
            visible = !visible;
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }
        SetRenderersVisible(true); // �Ō�͕K���\����Ԃɂ���
        blinkCoroutine = null;
    }

    private void SetRenderersVisible(bool visible)
    {
        foreach (var r in renderers)
        {
            r.enabled = visible;
        }
    }
}