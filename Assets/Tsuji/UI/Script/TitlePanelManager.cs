using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanelManager : MonoBehaviour
{
    public void OptionOpen()
    {
        OptionPanel.instance.Open();
    }
    public void OptionClose()
    {
        OptionPanel.instance.Close();
    }


}
