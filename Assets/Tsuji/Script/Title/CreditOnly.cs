using UnityEngine;
using UnityEngine.UI;

public class CreditOnly : MonoBehaviour
{
    [SerializeField] Button closeButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            closeButton.onClick.Invoke();
        }
    }
}
