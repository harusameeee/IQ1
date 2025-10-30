using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void ToSelect()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void ToPlay(string stageName)
    {
        SceneManager.LoadScene(stageName);
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
