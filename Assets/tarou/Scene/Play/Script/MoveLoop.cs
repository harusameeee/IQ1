using UnityEngine;
using UnityEngine.Splines;

public class MoveLoop : MonoBehaviour
{
    public float speed = 2.0f; // 通晨の移動速度

    public Rigidbody rb;
    
    public SplineContainer splinecont;
    NativeSpline spline;

    private float defaultSpeed; // 揄期速度保存用
    public float current_t_normalized = 0f;//レベル0からレベル1までの拱捏暎況を表す
    public float current_t=> current_t_normalized * spline.GetLength();
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        defaultSpeed = speed;
    }

    public virtual void Update()
    {
        spline = new NativeSpline(splinecont.Spline);
        var dist = SplineUtility.GetNearestPoint(spline, transform.position, out var nearest, out current_t_normalized);

        transform.position = Vector3.LerpUnclamped(transform.position, nearest, 0.4f);
        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(current_t_normalized));
        Vector3 up = spline.EvaluateUpVector(current_t_normalized);
        Vector3 euler = Quaternion.LookRotation(forward, up).eulerAngles;
        transform.localRotation = Quaternion.LerpUnclamped(Quaternion.Euler(new Vector3(0, euler.y, 0)), transform.localRotation, 0.5f);
        var newforward = transform.forward;

        // 移動揶理
        rb.linearVelocity = rb.linearVelocity.magnitude * 0.7f * newforward + newforward * speed;
    }
    public Vector4 getobstaclespawnpos(float offsetval, float dist, out bool valid,out float New_t)
    {
        valid = true;
        spline = new NativeSpline(splinecont.Spline);
        New_t = current_t_normalized + dist / spline.GetLength();
        if (New_t > 1.0f)
        {
            valid = false;
            return Vector3.zero;
        }
        Vector3 pos = spline.EvaluatePosition(New_t);
        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(New_t));
        Vector3 up = spline.EvaluateUpVector(New_t);
        New_t *= spline.GetLength();
        pos += Vector3.Cross(forward, up).normalized * offsetval;
        Vector4 temp =pos;
        temp.w=Quaternion.LookRotation(forward, up).eulerAngles.y;
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