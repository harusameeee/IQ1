using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanelManager : MonoBehaviour
{
    public void OptionOpen()
    {
        VolumeOption.instance.Open();
    }
    public void OptionClose()
    {
        VolumeOption.instance.Close();
    }


}
