using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    public Transform playerA;
    public Transform playerB;
    public int segmentCount = 10; // 頂点の数
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segmentCount;
    }

    void Update()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            float t = (float)i / (segmentCount - 1);
            // 両端間を補間
            Vector3 pos = Vector3.Lerp(playerA.position, playerB.position, t);
            // たるみ（例: Y軸方向にsinでカーブ）
            pos.y -= Mathf.Sin(Mathf.PI * t) * 0.2f;
            lineRenderer.SetPosition(i, pos);
        }
    }
}