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
    public float current_t = 0f;
    public static MoveLoop instance;
    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>(); 
        defaultSpeed = speed;
    }

    void Update()
    {
        spline = new NativeSpline(splinecont.Spline);
        var dist = SplineUtility.GetNearestPoint(spline, transform.position, out var nearest, out current_t);

        transform.position = Vector3.Lerp(transform.position, nearest, 0.2f);
        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(current_t));
        Vector3 up = spline.EvaluateUpVector(current_t);
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(new Vector3(0, Quaternion.LookRotation(forward, up).eulerAngles.y, 0)), transform.rotation, 0.2f);
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
    public Vector3 getobstaclespawnpos(float offsetval, float dist,out bool valid)
    {
        valid =true;
        spline = new NativeSpline(splinecont.Spline);
        float New_t = current_t + dist / spline.GetLength();
        if(New_t>1.0f)
        {
            valid = false;
            return Vector3.zero;
        }
        Vector3 pos = spline.EvaluatePosition(New_t);
        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(New_t));
        Vector3 up = spline.EvaluateUpVector(New_t);
        pos += Vector3.Cross(forward, up).normalized * offsetval;
        return pos;
    }
    public float get_dist(Vector3 pos1, Vector3 pos2)
    {
        spline = new NativeSpline(splinecont.Spline);
        SplineUtility.GetNearestPoint(spline, pos1, out var nearest1, out var t1);
        SplineUtility.GetNearestPoint(spline, pos2, out var nearest2, out var t2);       
        return Mathf.Abs(t2 - t1) * spline.GetLength();
    }
}