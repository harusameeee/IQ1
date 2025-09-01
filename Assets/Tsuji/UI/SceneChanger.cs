using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    void ToSelect()
    {
        SceneManager.LoadScene("SelectScene");
    }

    void ToPlay()
    {
        SceneManager.LoadScene("PlayScene");
    }

    void ToResult()
    {
        SceneManager.LoadScene("ResultScene");
    }

    void ToEnd()
    {
        Application.Quit();
    }

    void OpenOption()
    {
        //オプションを呼び出す
    }
}
