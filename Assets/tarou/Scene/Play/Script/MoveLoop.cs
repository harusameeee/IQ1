using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class MoveLoop : MonoBehaviour
{
    public float speed = 2.0f;
    public Rigidbody rb;

    public SplineContainer splinecont;
    protected NativeSpline spline;

    protected float defaultSpeed;

    // Current spline position (0-1)
    public float current_t_normalized = 0f;

    // Distance along spline
    public float current_t => current_t_normalized * spline.GetLength();

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultSpeed = speed;
    }

    public virtual void Update()
    {
        spline = new NativeSpline(splinecont.Spline);

        // Advance t based on speed (t-driven movement)
        float splineLength = spline.GetLength();
        float deltaT = (speed / splineLength) * Time.deltaTime;

        current_t_normalized += deltaT;
        current_t_normalized = Mathf.Clamp01(current_t_normalized);

        // Target position is always from spline
        Vector3 targetPos =
            spline.EvaluatePosition(current_t_normalized);

        // Optional height offset via FloatZone
        if (TryGetFloatZone(current_t_normalized, out FloatZone zone))
        {
            targetPos.y += zone.heightOffset;
        }

        // Smooth movement
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            0.25f
        );

        // Rotation from spline tangent
        Vector3 forward =
            Vector3.Normalize(
                spline.EvaluateTangent(current_t_normalized)
            );

        Vector3 up =
            spline.EvaluateUpVector(current_t_normalized);

        Quaternion targetRot =
            Quaternion.LookRotation(forward, up);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRot,
            0.25f
        );

        // Rigidbody forward velocity
        Vector3 moveForward = transform.forward;

        rb.linearVelocity =
            rb.linearVelocity.magnitude * 0.7f * moveForward +
            moveForward * speed;
    }

    // Get a position ahead on the spline
    public Vector4 getobstaclespawnpos(
        float offsetval,
        float dist,
        out bool valid,
        out float new_t
    )
    {
        valid = true;

        spline = new NativeSpline(splinecont.Spline);

        float length = spline.GetLength();

        new_t = current_t_normalized + dist / length;

        if (new_t > 1.0f)
        {
            valid = false;
            return Vector3.zero;
        }

        Vector3 pos = spline.EvaluatePosition(new_t);
        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(new_t));
        Vector3 up = spline.EvaluateUpVector(new_t);

        pos += Vector3.Cross(forward, up).normalized * offsetval;

        Vector4 result = pos;
        result.w = Quaternion.LookRotation(forward, up).eulerAngles.y;

        return result;
    }

    // Distance along spline between two points
    public float get_dist(Vector3 pos1, Vector3 pos2)
    {
        spline = new NativeSpline(splinecont.Spline);

        float3 _;
        float t1;
        float t2;

        SplineUtility.GetNearestPoint(
            spline,
            new Ray(pos1, Vector3.down),
            out _,
            out t1
        );

        SplineUtility.GetNearestPoint(
            spline,
            new Ray(pos2, Vector3.down),
            out _,
            out t2
        );

        return Mathf.Abs(t2 - t1) * spline.GetLength();
    }

    // Speed zones
    [System.Serializable]
    public class SpeedZone
    {
        public float startT01;
        public float endT01;
        public float speedMultiplier = 1f;
    }

    public SpeedZone[] speedZones;

    protected float GetSpeedMultiplier(float t01)
    {
        if (speedZones == null)
            return 1f;

        foreach (var z in speedZones)
        {
            if (t01 >= z.startT01 && t01 <= z.endT01)
                return z.speedMultiplier;
        }

        return 1f;
    }

    void LateUpdate()
    {
        speed = defaultSpeed * GetSpeedMultiplier(current_t_normalized);
    }

    // Float zones (height adjustment only)
    [System.Serializable]
    public class FloatZone
    {
        public float startT01;
        public float endT01;
        public float heightOffset;
    }

    public FloatZone[] floatZones;

    protected bool TryGetFloatZone(float t01, out FloatZone zone)
    {
        if (floatZones != null)
        {
            foreach (var z in floatZones)
            {
                if (t01 >= z.startT01 && t01 <= z.endT01)
                {
                    zone = z;
                    return true;
                }
            }
        }

        zone = null;
        return false;
    }
}
