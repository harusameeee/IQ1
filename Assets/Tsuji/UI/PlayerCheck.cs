using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCheck : MonoBehaviour
{
    //　準備OK
    [SerializeField] Image isReady;
    //
    StageSelect stageSelect;

    // Update is called once per frame
    void Update()
    {
        //選択状態のときのみ
        if(stageSelect.isSelect)
        {
            //表示させる

            //プレイヤーの入力


        }
    }
}
