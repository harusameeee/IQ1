using UnityEngine;

public class Attack5 : MonoBehaviour
{
    public GameObject shadowPrefab;
    public float groundY = 0f;
    public float waitTime = 1f;
    public float spawnHeight = 5.0f; // 何単位上に出すか

    private GameObject shadowInstance;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.useGravity = false;

        // プレイヤーオブジェクトを探す
        GameObject[] targets = new GameObject[2];
        targets[0] = GameObject.FindWithTag("Player");
        targets[1] = GameObject.FindWithTag("Player2");

        GameObject target = null;
        if (targets[0] != null && targets[1] != null)
        {
            target = targets[Random.Range(0, 2)];
        }
        else if (targets[0] != null)
        {
            target = targets[0];
        }
        else if (targets[1] != null)
        {
            target = targets[1];
        }

        // ターゲットの真上に出現
        if (target != null)
        {
            Vector3 spawnPos = target.transform.position + Vector3.up * spawnHeight;
            transform.position = spawnPos;
        }

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

        if (transform.position.y < groundY)
        {
            Destroy(shadowInstance);
            Destroy(gameObject);
        }
    }
}