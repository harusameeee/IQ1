using UnityEngine;

public class FireMagic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            TriggerEvent();
        }
    }

    private void TriggerEvent()
    {
        Destroy(gameObject);
        HPEvent();
    }


    private void HPEvent()
    {
        
    }
}
