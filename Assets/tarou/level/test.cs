using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class test : MonoBehaviour
{
    public SplineContainer splineContainer;
    public LayerMask groundLayer;

    void Start()
    {
        if (splineContainer == null)
        {
            Debug.LogError("SplineContainer not assigned!");
            return;
        }
        calibrate_raycast();

    }
    public void offset()
    {
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < spline.Count; i++)
        {
            BezierKnot currentKnot = spline[i];
            currentKnot.Position += (float3)this.transform.position;
            spline[i] = currentKnot;
        }   
    }
    public void calibrate_raycast()
    {
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < spline.Count; i++)
        {
            BezierKnot currentKnot = spline[i];
            Physics.Raycast((Vector3)currentKnot.Position + Vector3.up * 3f, Vector3.down, out RaycastHit hitInfo, 50f, groundLayer);
            if(hitInfo.collider != null)
            {
                currentKnot.Position = (float3)hitInfo.point;
                 spline[i] = currentKnot;
            }
            else
            {
             Debug.LogWarning($"Raycast did not hit ground for knot {i} at position {currentKnot.Position}");   
            }
           
        }
    }
}
