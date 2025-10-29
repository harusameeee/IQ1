using Cysharp.Threading.Tasks;
using UnityEngine;

public class ElectricityMagic : MonoBehaviour
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
