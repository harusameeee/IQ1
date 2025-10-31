using TMPro;
using UnityEngine;

public class jobText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_TextMeshPro;

    [SerializeField] SelectedPlayerJob job;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_TextMeshPro.text=job.playerJobName.ToString();
    }
}
