using UnityEngine;

public class Attack5 : MonoBehaviour
{
    public GameObject shadowPrefab;     // ←これをプレハブのInspectorで事前にセット

    public float groundY = 0f;
    public float waitTime = 1f;

    private GameObject shadowInstance;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.useGravity = false;

        // 影を生成
        if (shadowPrefab != null)
        {
            Vector3 shadowPos = new Vector3(transform.position.x, groundY + 0.01f, transform.position.z);
            shadowInstance = Instantiate(shadowPrefab, shadowPos, Quaternion.identity);
        }

        Invoke(nameof(StartFalling), waitTime);
    }

    void StartFalling()
    {
        if (rb != null) rb.useGravity = true;
    }

    void Update()
    {
        if (shadowInstance != null)
        {
            shadowInstance.transform.position = new Vector3(transform.position.x, groundY + 0.01f, transform.position.z);
        }

        if (transform.position.y < groundY - 1f)
        {
            if (shadowInstance != null) Destroy(shadowInstance);
            Destroy(gameObject);
        }
    }
}