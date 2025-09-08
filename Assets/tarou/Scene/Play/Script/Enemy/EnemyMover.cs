using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public float moveDistance = 5f;     // Z•ûŒü‚É‚Ç‚ê‚¾‚¯“®‚©‚·‚©
    public float moveSpeed = 3f;        // ˆÚ“®‘¬“x
    private bool shouldMove = false;
    private Vector3 targetPosition;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player‚ÆÚG");
            shouldMove = true;
            targetPosition = transform.position + Vector3.forward * moveDistance;
        }
    }

    void Update()
    {
        if (shouldMove)
        {
            // Z•ûŒü‚É™X‚ÉˆÚ“®
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // ˆÚ“®I—¹‚µ‚½‚ç~‚ß‚é
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                shouldMove = false;
            }
        }
    }
}
