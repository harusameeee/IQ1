    using UnityEngine;
using UnityEngine.UI;

public class bufficon : MonoBehaviour
{
    public Image buffimg;
    public TMPro.TMP_Text stacktext;
    public TMPro.TMP_Text durationtext;
    
    [SerializeReference]
    public buffdata referencedbuff;

    void Update()
    {
        if (referencedbuff == null)
        {
            return;
        }
        stacktext.text = referencedbuff.pow.ToString();
        durationtext.text = referencedbuff.duration.ToString("F1");
    }
}
