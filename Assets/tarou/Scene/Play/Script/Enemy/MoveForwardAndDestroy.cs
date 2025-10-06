using UnityEngine;

public class MoveForwardAndDestroy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float destroyZ = -20f;

    void Update()
    {
        // Z??????
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.Self);

        // ‰æ–ÊŠO‚És‚Á‚½‚ç”jŠü
        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }
}
