using UnityEngine;

public class CameraRotatorOnHit : MonoBehaviour
{
    public float rotationSpeed = 90f; // 回転速度（度/秒）

    private Camera targetCamera;
    private RectTransform uiRoot;
    private bool rotating = false;
    private float targetZRotation;
    private float uiTargetZRotation;

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

        // タグ "RotatableUI" のUIを探す
        GameObject uiObj = GameObject.FindGameObjectWithTag("UI");
        if (uiObj != null)
        {
            uiRoot = uiObj.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogWarning("タグ 'UI'が見つかりません");
        }
    }

    void Update()
    {
        if (rotating)
        {
            // --- カメラを回す ---
            if (targetCamera != null)
            {
                float currentZ = targetCamera.transform.eulerAngles.z;
                float newZ = Mathf.MoveTowardsAngle(currentZ, targetZRotation, rotationSpeed * Time.deltaTime);

                Vector3 euler = targetCamera.transform.eulerAngles;
                targetCamera.transform.eulerAngles = new Vector3(euler.x, euler.y, newZ);
            }

            // --- UIを回す ---
            if (uiRoot != null)
            {
                float currentZUI = uiRoot.eulerAngles.z;
                float newZUI = Mathf.MoveTowardsAngle(currentZUI, uiTargetZRotation, rotationSpeed * Time.deltaTime);

                Vector3 eulerUI = uiRoot.eulerAngles;
                uiRoot.eulerAngles = new Vector3(eulerUI.x, eulerUI.y, newZUI);
            }

            // 両方が到達したら終了
            bool cameraDone = (targetCamera == null) || Mathf.Approximately(targetCamera.transform.eulerAngles.z, targetZRotation);
            bool uiDone = (uiRoot == null) || Mathf.Approximately(uiRoot.eulerAngles.z, uiTargetZRotation);

            if (cameraDone && uiDone)
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
        }

        if (uiRoot != null)
        {
            float currentZUI = uiRoot.eulerAngles.z;
            uiTargetZRotation = currentZUI + 180f;
        }

        rotating = true;
    }
}
