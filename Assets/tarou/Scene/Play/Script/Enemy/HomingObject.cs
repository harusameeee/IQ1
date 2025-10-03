using UnityEngine;

public class HomingObject : MonoBehaviour
{
    public float speed = 10f;  // ��ԑ��x
    public float destroyZ = -20f;

    private Transform target;

    void Start()
    {
        // "Player"��"Player2"�^�O�������I�u�W�F�N�g��T��
        GameObject player1 = GameObject.FindGameObjectWithTag("Player");
        GameObject player2 = GameObject.FindGameObjectWithTag("Player2");

        // �ǂ��炩�����݂���΁A�����_���Ń^�[�Q�b�g�ɂ���
        GameObject[] candidates = new GameObject[] { player1, player2 };
        candidates = System.Array.FindAll(candidates, go => go != null);

        if (candidates.Length > 0)
        {
            GameObject chosen = candidates[Random.Range(0, candidates.Length)];
            target = chosen.transform;

            // Rigidbody �ɑ��x��ݒ肵�ăv���C���[�̕����ɔ�΂�
            Vector3 direction = (target.position - transform.position).normalized;
            GetComponent<Rigidbody>().linearVelocity = direction * speed;
        }
        else
        {
            Debug.LogWarning("�v���C���[��������܂���ł���");
        }
    }

    void Update()
    {
        // ��ʊO�ɍs������j��
        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }
}