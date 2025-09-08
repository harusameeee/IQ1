using UnityEngine;

public class MutualRopeLimiter : MonoBehaviour
{
    public Transform playerA;
    public Transform playerB;
    public float maxDistance = 5f;

    void Update()
    {
        Vector3 diff = playerA.position - playerB.position;
        float distance = diff.magnitude;

        if (distance > maxDistance)
        {
            Vector3 mid = (playerA.position + playerB.position) / 2;
            Vector3 offset = diff.normalized * (maxDistance / 2);
            playerA.position = mid + offset;
            playerB.position = mid - offset;
        }
    }
}