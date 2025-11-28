using UnityEngine;
using UnityEngine.Splines;

public class MoveLoop : MonoBehaviour
{
    public float speed = 2.0f;                    // base movement speed
    public Rigidbody rb;

    public SplineContainer splinecont;
    private NativeSpline spline;

    private float defaultSpeed;                   // store initial speed

    public float current_t_normalized = 0f;       // normalized spline position (0-1)
    public float current_t => current_t_normalized * spline.GetLength();

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultSpeed = speed;
    }

    public virtual void Update()
    {
        spline = new NativeSpline(splinecont.Spline);

        // get nearest point on spline
        SplineUtility.GetNearestPoint(
            spline,
            transform.position,
            out var nearest,
            out current_t_normalized
        );

        // raycast to ground
        Physics.Raycast(
            transform.position + Vector3.up * 3f,
            Vector3.down,
            out RaycastHit hitInfo,
            50f,
            LayerMask.GetMask("lvll")
        );

        if (hitInfo.collider != null)
        {
            nearest.y = hitInfo.point.y;
        }

        // move toward spline point
        transform.position = Vector3.LerpUnclamped(
            transform.position,
            nearest,
            0.4f
        );

        // orientation from spline tangent
        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(current_t_normalized));
        Vector3 up = spline.EvaluateUpVector(current_t_normalized);

        Vector3 euler = Quaternion.LookRotation(forward, up).eulerAngles;

        transform.localRotation = Quaternion.LerpUnclamped(
            Quaternion.Euler(0f, euler.y, 0f),
            transform.localRotation,
            0.5f
        );

        // final movement
        Vector3 newforward = transform.forward;

        rb.linearVelocity =
            rb.linearVelocity.magnitude * 0.7f * newforward +
            newforward * speed;
    }

    public Vector4 getobstaclespawnpos(
        float offsetval,
        float dist,
        out bool valid,
        out float new_t
    )
    {
        valid = true;

        spline = new NativeSpline(splinecont.Spline);

        new_t = current_t_normalized + dist / spline.GetLength();

        if (new_t > 1.0f)
        {
            valid = false;
            return Vector3.zero;
        }

        Vector3 pos = spline.EvaluatePosition(new_t);
        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(new_t));
        Vector3 up = spline.EvaluateUpVector(new_t);

        new_t *= spline.GetLength();

        pos += Vector3.Cross(forward, up).normalized * offsetval;

        Vector4 temp = pos;
        temp.w = Quaternion.LookRotation(forward, up).eulerAngles.y;

        return temp;
    }

    public float get_dist(Vector3 pos1, Vector3 pos2)
    {
        spline = new NativeSpline(splinecont.Spline);

        // get spline t for both positions
        SplineUtility.GetNearestPoint(spline, pos1, out _, out float t1);
        SplineUtility.GetNearestPoint(spline, pos2, out _, out float t2);

        return Mathf.Abs(t2 - t1) * spline.GetLength();
    }

    // -------------------------
    // speed zone extension
    // -------------------------

    [System.Serializable]
    public class SpeedZone
    {
        public float startT01 = 0f;       // zone start (0-1)
        public float endT01 = 0.1f;       // zone end (0-1)
        public float speedMultiplier = 1.5f;
    }

    public SpeedZone[] speedZones;

    private float GetSpeedMultiplier(float t01)
    {
        if (speedZones == null || speedZones.Length == 0)
            return 1f;

        foreach (var z in speedZones)
        {
            if (t01 >= z.startT01 && t01 <= z.endT01)
                return z.speedMultiplier;
        }
        return 1f;
    }

    private void LateUpdate()
    {
        float t01 = current_t_normalized;

        float mul = GetSpeedMultiplier(t01);

        speed = defaultSpeed * mul;
    }
}
