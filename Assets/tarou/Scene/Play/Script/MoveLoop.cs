using UnityEngine;
using UnityEngine.Splines;

public class MoveLoop : MonoBehaviour
{
    public float speed = 2.0f; // ’Êí‚ÌˆÚ“®‘¬“x

    public Rigidbody rb;
    
    public SplineContainer splinecont;
    NativeSpline spline;

    private float defaultSpeed; // ‰Šú‘¬“x•Û‘¶—p
<<<<<<< Updated upstream

    void Start()
=======
    public float current_t = 0f;
    public static MoveLoop instance;
    public virtual void Start()
>>>>>>> Stashed changes
    {
        rb = GetComponent<Rigidbody>(); 
        defaultSpeed = speed;
    }

    public virtual void Update()
    {
        spline = new NativeSpline(splinecont.Spline);
<<<<<<< Updated upstream
        var dist = SplineUtility.GetNearestPoint(spline, transform.position, out var nearest, out var t);
        Debug.Log(transform.position+"and"+nearest);
        transform.position = nearest;
        var forward = Vector3.Normalize(spline.EvaluateTangent(t));
        Vector3 up = spline.EvaluateUpVector(t);
        transform.rotation = Quaternion.Euler(new Vector3(0, Quaternion.LookRotation(forward, up).eulerAngles.y, 0));
=======
        var dist = SplineUtility.GetNearestPoint(spline, transform.position, out var nearest, out current_t);

        transform.position = Vector3.LerpUnclamped(transform.position, nearest, 0.4f);
        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(current_t));
        Vector3 up = spline.EvaluateUpVector(current_t);
        transform.rotation = Quaternion.LerpUnclamped(Quaternion.Euler(new Vector3(0, Quaternion.LookRotation(forward, up).eulerAngles.y, 3)), transform.rotation, 0.2f);
>>>>>>> Stashed changes
        var newforward = transform.forward;

        // ˆÚ“®ˆ—
        rb.linearVelocity = rb.linearVelocity.magnitude * 0.7f * newforward + newforward * speed;
    }
<<<<<<< Updated upstream
=======
    public Vector3 getobstaclespawnpos(float offsetval, float dist, out bool valid)
    {
        valid = true;
        spline = new NativeSpline(splinecont.Spline);
        float New_t = current_t + dist / spline.GetLength();
        if (New_t > 1.0f)
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
>>>>>>> Stashed changes
}