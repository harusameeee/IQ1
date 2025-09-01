using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Option : MonoBehaviour
{
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject[] buttons;

    void OpenOption()
    { 

        optionPanel.SetActive(true);
        foreach (var button in buttons)
        {

        }
    }

    void CloseOption()
    {
        optionPanel.SetActive(false);
    }
}
