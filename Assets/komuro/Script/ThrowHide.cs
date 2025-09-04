using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHide : MonoBehaviour
{
    // ターゲットに到達するまでの時間
    [SerializeField] float moveTime = 1.5f;
    // カメラの座標
    private Camera cam;
    private Transform target;
    // 最初の座標
    private Vector3 startPos;
    private Vector3 targetPos;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        GameObject mainCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        cam = mainCameraObj.GetComponent<Camera>();
        target = cam.transform;
        targetPos = target.position;
        startPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // カメラに向かわせる
        timer += Time.deltaTime;
        float times = Mathf.Clamp01(timer / moveTime);

        // 始点→終点を直線補間（時間基準）
        transform.position = Vector3.Lerp(startPos, targetPos, times);

        if (times >= 1f)
        {
            // カメラにぶつかったら視界を悪くする

            Destroy(gameObject);
        }

    }
}
