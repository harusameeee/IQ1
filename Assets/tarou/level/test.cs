using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class test : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float3 translationOffset = new float3(1f, 0f, 0f);

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
            currentKnot.Position += translationOffset;
            spline[i] = currentKnot;
        }

    }
}
