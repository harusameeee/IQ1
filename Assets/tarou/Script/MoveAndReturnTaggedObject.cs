using UnityEngine;
using System.Collections;

public class MoveAndReturnTaggedObject : MonoBehaviour
{
    public string targetTag = "EnemyCharacter"; // “®‚©‚µ‚½‚¢ƒ^ƒO
    public float moveDistance = 1f;             // i‚ß‚é‹——£

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

            foreach (GameObject target in targets)
            {
                Vector3 startPos = target.transform.position;
                Vector3 movedPos = startPos + new Vector3(0, 0, moveDistance);

                // ‚·‚®ˆÚ“®
                target.transform.position = movedPos;

            }
        }
    }

   
}
