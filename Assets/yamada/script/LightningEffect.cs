using UnityEngine;

public class LightningEffect : MonoBehaviour
{

    public int pointsCount = 64;// 波を作るギザギザの数
    public float baseNoiseStrength = 0.5f;// ギザギザの波の高さ(強さ)
    public float baseNoiseSpeed = 6f;// 波変形をさせるスピード
    public float flashInterval = 0.2f; // 何秒ごとに雷が強く変化するか

    private LineRenderer lineRenderer;
    private float[] randomOffsets;
    private float lastFlashTime;
    [SerializeField] GameObject StartPos;
    [SerializeField] GameObject EndPos;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointsCount;
        lineRenderer.widthMultiplier = 0.12f;

        // 雷色グラデーション
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(new Color(0.9f, 0.9f, 1f), 0.0f), // 青白
                new GradientColorKey(new Color(0.4f, 0.7f, 1f), 0.7f), // 青
                new GradientColorKey(new Color(1f, 1f, 1f), 1.0f)      // 白
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(1.0f, 1.0f)
            }
        );
        lineRenderer.colorGradient = gradient;

        // 頂点ごとのノイズオフセット（ギザギザパターンのランダム化用）
        randomOffsets = new float[pointsCount];

        ResetRandomOffsets();
        lastFlashTime = Time.time;
    }

    void Update()
    {
        // 一定間隔でランダム形状をリセット（雷の激しさを強調）
        if (Time.time - lastFlashTime > flashInterval)
        {
            ResetRandomOffsets();
            lastFlashTime = Time.time;
        }

        float noiseSpeed = baseNoiseSpeed;
        float noiseStrength = baseNoiseStrength;

        for (int i = 0; i < pointsCount; i++)
        {
            float t = (float)i / (pointsCount - 1);

            //開始と終了地点のオブジェクトのポジション同士で補間を掛ける
            Vector3 pos = Vector3.Lerp(StartPos.transform.position,EndPos.transform.position,t);

            // Perlinノイズ＋ランダムオフセットで、激しいギザギザ
            float time = Time.time * noiseSpeed;
            float offsetX = Mathf.PerlinNoise(i * 0.25f + time + randomOffsets[i], 0) - 0.5f;
            float offsetY = Mathf.PerlinNoise(i * 0.25f, time + randomOffsets[i]) - 0.5f;

            // 雷は主にXY方向に激しく揺れる
            Vector3 noise = new Vector3(offsetX, offsetY, 0) * noiseStrength;
            Vector3 defaultNoise = new Vector3(offsetX, offsetY, 0);

            // 中央付近ほど揺れを強く
            float centerWeight = Mathf.Sin(Mathf.PI * t);
            pos += noise * centerWeight;

            lineRenderer.SetPosition(i, pos);
        }
    }

    // 雷形状の激しい変化用乱数
    void ResetRandomOffsets()
    {
        for (int i = 0; i < pointsCount; i++)
        {
            randomOffsets[i] = Random.Range(0f, 100f);
        }
    }
}