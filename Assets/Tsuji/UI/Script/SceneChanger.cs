using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ToSelect()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void ToPlay()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void ToResult()
    {
        SceneManager.LoadScene("ResultScene");
    }

    public void ToEnd()
    {
        Application.Quit();
    }

}
