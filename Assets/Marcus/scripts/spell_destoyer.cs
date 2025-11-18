using UnityEngine;

public class spell_destoyer : MonoBehaviour
{
    public Transform target_transform;
void destroyspells()
    {
        foreach(Transform child in target_transform)
        {
            Destroy(child.gameObject);
        }
    }
}
