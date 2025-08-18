using UnityEngine;

public class CameraRotatorOnHit : MonoBehaviour
{
    public float rotationSpeed = 90f; // 回転速度（度/秒）

    private Camera targetCamera;
    private bool rotating = false;
    private float targetZRotation;

    void Start()
    {
        // タグ "DefaultCamera" のついたカメラを探す
        GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObj != null)
        {
            targetCamera = cameraObj.GetComponent<Camera>();
        }
        else
        {
            Debug.LogError("タグ 'DefaultCamera' のついたカメラが見つかりません");
        }
    }

    void Update()
    {
        if (rotating && targetCamera != null)
        {
            float currentZ = targetCamera.transform.eulerAngles.z;
            float newZ = Mathf.MoveTowardsAngle(currentZ, targetZRotation, rotationSpeed * Time.deltaTime);

            Vector3 euler = targetCamera.transform.eulerAngles;
            targetCamera.transform.eulerAngles = new Vector3(euler.x, euler.y, newZ);

            if (Mathf.Approximately(newZ, targetZRotation))
            {
                rotating = false;
            }
        }
    }

    // 衝突時に回転スタート
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // または条件を変更して自由に
        {
            StartRotation();
        }
    }

    void StartRotation()
    {
        if (targetCamera != null)
        {
            float currentZ = targetCamera.transform.eulerAngles.z;
            targetZRotation = currentZ + 180f;
            rotating = true;
        }
    }
}
