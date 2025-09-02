//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 60FPSに制限を与える
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {

        //Escが押された時
        if (Input.GetKeyDown("joystick button 6"))
        {
            Debug.Log("ボタンが押された");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }
    }
}
