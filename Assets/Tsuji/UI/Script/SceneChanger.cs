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
        SceneManager.LoadScene("Stage1");
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
