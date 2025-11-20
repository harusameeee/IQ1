using UnityEngine;
using UnityEngine.UI;

public class barfill : MonoBehaviour
{
    public Image img;
    public float fill_val;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        img = GetComponent<Image>();
        img.type = Image.Type.Filled;
        img.fillMethod = Image.FillMethod.Horizontal;
    }

    // Update is called once per frame
    void Update()
    {
        img.fillAmount = fill_val;
    }
}
