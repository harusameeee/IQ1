using UnityEngine;

public class transition_trigger_test : MonoBehaviour
{
    public Animation transition_anim;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);
        if(other.CompareTag("Player")||other.CompareTag("Player2"))
        {
            
            transition_anim.Play();
        }
    }
}
