using UnityEngine;
using UnityEngine.Splines;

public class MoveLoopDebug : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    public float speed = 2.0f; // �ʏ�̈ړ����x

    [Header("�v���C���[�Q��")]
    public PlayerLineMove player; // PlayerLineMove�Q�Ɨp
    public PlayerLineMove player2; // PlayerLineMove�Q�Ɨp
    public Rigidbody rb;

    [SerializeField] private SplineContainer splinecont;
    NativeSpline spline;

    private float defaultSpeed; // �������x�ۑ��p

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultSpeed = speed;
    }

    void Update()
    {
        // NativeSpline�̐�����Start���ł���Ă��ǂ�
        spline = new NativeSpline(splinecont.Spline);
        var dist = SplineUtility.GetNearestPoint(spline, transform.position, out var nearest, out var t);

        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(t));
        Vector3 up = spline.EvaluateUpVector(t);
        Vector3 right = Vector3.Normalize(Vector3.Cross(up, forward)); // �C��

        int lane = player != null ? player.currentLane : 0;
       // float laneOffset = player != null ? player.laneOffset : 4f;
        //float jumpOffset = player != null ? player.jumpOffset : 0f;

        Vector3 nearestV3 = new Vector3(nearest.x, nearest.y, nearest.z);
        //Vector3 targetPos = nearestV3 + right * (lane * laneOffset) + Vector3.up * jumpOffset;

        // �����œ������Ȃ炱����
        //rb.MovePosition(targetPos);

        // ��]
        transform.rotation = Quaternion.Euler(new Vector3(0, Quaternion.LookRotation(forward, up).eulerAngles.y, 0));

        // ���x�t�^�i�����ŊǗ�����ꍇ�̂݁j
        rb.linearVelocity = forward * speed / 2f;
    }
}