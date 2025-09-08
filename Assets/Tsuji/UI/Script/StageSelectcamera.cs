using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectcamera : MonoBehaviour
{
    //playerに追従させたい
    [SerializeField] GameObject player;
    // 速度
    Vector3 velocity = Vector3.zero;
    // カメラ位置
    [SerializeField] Vector3 position = new Vector3(0, 3, -6);

    void LateUpdate()
    {
        transform.position = 
            Vector3.SmoothDamp(transform.position,
            player.transform.position + position,
            ref velocity, 0.3f);	// カメラを少し遅れて移動させる処理
    }
}
