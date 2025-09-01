using UnityEngine;
using TMPro;

public class PlayerLabel : MonoBehaviour
{
    public Transform playerTransform;
    public TextMeshProUGUI labelText;
    public Vector3 offset = new Vector3(0, 2, 0);

    void Update()
    {
        Vector3 worldPosition = playerTransform.position + offset;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        labelText.transform.position = screenPosition;
    }
}