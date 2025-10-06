using UnityEngine;
using UnityEngine.Splines;

public class MoveLoop : MonoBehaviour
{
    [Header("移動設定")]
    public float speed = 2.0f; // 通常の移動速度

    [Header("プレイヤー参照")]
    public Player3DController player; // Player3DController参照用
    public Player3DController player2; // Player3DController参照用
    public Rigidbody rb;
    
    [SerializeField] private SplineContainer splinecont;
    NativeSpline spline;

    private float defaultSpeed; // 初期速度保存用

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        defaultSpeed = speed;
    }

    void Update()
    {
        spline = new NativeSpline(splinecont.Spline);
        var dist = SplineUtility.GetNearestPoint(spline, transform.position, out var nearest, out var t);
        Debug.Log(transform.position+"and"+nearest);
        transform.position = nearest;
        var forward = Vector3.Normalize(spline.EvaluateTangent(t));
        Vector3 up = spline.EvaluateUpVector(t);
        transform.rotation = Quaternion.Euler(new Vector3(0, Quaternion.LookRotation(forward, up).eulerAngles.y, 0));
        var newforward = transform.forward;
        if ((player != null && player.currentState == Player3DController.State.Slow) ||
            (player2 != null && player2.currentState == Player3DController.State.Slow))
        {
            speed = 1.5f;
        }
        else
        {
            speed = defaultSpeed;
        }
        // 移動処理
        rb.linearVelocity = rb.linearVelocity.magnitude * 0.7f * newforward + newforward * speed;
    }
}