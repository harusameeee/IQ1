using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerLineMove : MonoBehaviour
{
    // ���[����X���W�i��F���E�����E�E�j
    // X positions for each lane (e.g., left, center, right)
    public float[] linePositions = { -8f, -4f, 0f, 4f, 8f };

    // ���݂��郌�[���i�����l�͒����j
    // The current lane index (starts at center lane)
    public int currentLane = 1;

    // �ʏ�ړ����x
    // Normal movement speed
    public float moveSpeed = 5f;

    // �X���[��Ԏ��̈ړ����x
    // Movement speed when in "Slow" state
    public float slowMoveSpeed = 2f;

    // �W�����v��
    // Jump force
    public float jumpForce = 7f;

    // �n�ʔ���p���C���[�}�X�N
    // LayerMask for ground detection
    public LayerMask groundLayer;

    // �v���C���[�ԍ��i1�܂���2�j
    // Player number (1 or 2)
    public int playerNumber = 1;

    // �����ϐ�
    // Internal variables
    private Rigidbody rb;
    private bool isGrounded;

    // �v���C���[��Ԃ̒�`
    // Player state definition
    public enum State { Normal, Slow }
    public State currentState = State.Normal;

    // �R���[�`���p�ϐ�
    // Coroutine references
    private Coroutine slowCoroutine;
    private Coroutine blinkCoroutine;

    // �����_���[�擾�p
    // For getting all Renderers in the object
    private Renderer[] renderers;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // �q�I�u�W�F�N�g���܂ޑSRenderer�擾
        // Get all Renderers including children
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        // �n�ʔ���
        // Check if player is grounded
        isGrounded = CheckIfGrounded();

        // �v���C���[���Ƃ̃W�����v�{�^��
        // Jump button per player
        string jumpButton = playerNumber == 1 ? "joystick 1 button 0" : "joystick 2 button 0";
        if (Input.GetKeyDown(jumpButton) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }

        // ���[���ړ��i���E�{�^���j
        // Lane change (left/right button)
        // ジャンプしているときは移動できないようにする
        if (isGrounded)
        {
            string leftKey = playerNumber == 1 ? "joystick 1 button 4" : "joystick 2 button 4";
            string rightKey = playerNumber == 1 ? "joystick 1 button 5" : "joystick 2 button 5";
            if (Input.GetKeyDown(leftKey))
            {
                currentLane = Mathf.Max(0, currentLane - 1);
                MoveToLane();
            }
            if (Input.GetKeyDown(rightKey))
            {
                currentLane = Mathf.Min(linePositions.Length - 1, currentLane + 1);
                MoveToLane();
            }
        }
    }

    void FixedUpdate()
    {
        // �O�i���x�i�K�v�Ȃ�ύX�j
        // Forward movement speed (edit as needed)
        float speed = currentState == State.Normal ? moveSpeed : slowMoveSpeed;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, speed);
    }

    // ���[�����W�ɃX�i�b�v
    // Snap player to lane position
    void MoveToLane()
    {
        Vector3 pos = transform.position;
        pos.x = linePositions[currentLane];
        transform.position = pos;
    }

    // �n�ʔ���
    // Ground detection
    private bool CheckIfGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    // �G�U����J��������^�O�̏���
    // Handle collision with "EnemyAttack" or "magic camera" tags
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            if (slowCoroutine != null) StopCoroutine(slowCoroutine);
            slowCoroutine = StartCoroutine(SwitchToSlowAndReturn());

            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
            blinkCoroutine = StartCoroutine(Blink(1f, 0.1f));
        }

        if (other.CompareTag("magic camera"))
        {
            if (slowCoroutine != null) StopCoroutine(slowCoroutine);
            slowCoroutine = StartCoroutine(SwitchToSlowAndReturn());
        }
    }

    // �X���[��Ԃɂ��Ė߂�
    // Switch to Slow state for 1 second then return to Normal
    private IEnumerator SwitchToSlowAndReturn()
    {
        currentState = State.Slow;
        yield return new WaitForSeconds(1f);
        currentState = State.Normal;
        slowCoroutine = null;
    }

    // �v���C���[�̌����ڂ�_�ł�����
    // Blink player appearance
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
        SetRenderersVisible(true); // �Ō�͕\���ɖ߂�
        blinkCoroutine = null;
    }

    // �����_���[�̕\���؂�ւ�
    // Toggle visibility of all renderers
    private void SetRenderersVisible(bool visible)
    {
        foreach (var r in renderers)
        {
            r.enabled = visible;
        }
    }
}