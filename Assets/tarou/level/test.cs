using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class test : MonoBehaviour
{
    public SplineContainer splineContainer;

    void Start()
    {
        if (splineContainer == null)
        {
            Debug.LogError("SplineContainer not assigned!");
            return;
        }
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < spline.Count; i++)
        {
            BezierKnot currentKnot = spline[i];
            currentKnot.Position += (float3)this.transform.position;
            spline[i] = currentKnot;
        }

    }
}
