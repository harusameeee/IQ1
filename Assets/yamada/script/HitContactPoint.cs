using UnityEngine;

public class PlayerHitEffect : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private float destroyTime = 2f;
    [SerializeField] private string enemyAttack_TagName;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyAttack"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Vector3 contactPoint = contact.point;

                Vector3 dirToPlayer = (transform.position - contactPoint).normalized;

                Ray ray = new Ray(contactPoint - dirToPlayer * 0.1f, dirToPlayer);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 2f, LayerMask.GetMask("Player")))
                {
                    CreateHitEffect(hit.point); 
                }
                else
                {

                    CreateHitEffect(contactPoint);
                }

                break; 
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyAttack_TagName))
        {
            Vector3 hitPos = other.ClosestPoint(transform.position);
            CreateHitEffect(hitPos);
        }
    }


    private void CreateHitEffect(Vector3 position)
    {
        GameObject effect = Instantiate(hitEffectPrefab, position, Quaternion.identity);
        Destroy(effect, destroyTime);
    }
}
