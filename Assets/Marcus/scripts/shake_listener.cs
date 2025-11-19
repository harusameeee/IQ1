using Unity.Cinemachine;
using UnityEngine;

public class shake_listener : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;
    public void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        hitbox.dmg_dealt += playshake;
    }
    public void playshake(float intensity)
    {
        impulseSource.GenerateImpulse(intensity);
    }

}
