using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIControl : MonoBehaviour
{
    [SerializeField] GameObject firstButton;

    //選択状態を解除→変更
    public void ChangedSelectButton()
    {
        //初期選択ボタンの初期化
        EventSystem.current.SetSelectedGameObject(null);
        //初期選択ボタンの再指定
        EventSystem.current.SetSelectedGameObject(firstButton);
    }


}
