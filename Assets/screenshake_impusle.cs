using Unity.Cinemachine;
using UnityEngine;

public class screenshake_impusle : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;
    public void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        
    }
    public void playshake()
    {
        impulseSource.GenerateImpulse();
    }
    public void onEnable()
    {
        impulseSource.GenerateImpulse();
    }
}
