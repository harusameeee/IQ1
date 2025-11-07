using UnityEngine;

public class MagicCircleTrigger : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle1;
    [SerializeField] private ParticleSystem particle2;

    public bool isPlaying = false;

    void Awake()
    {
        // Inspectorで設定されていない場合、自動で子から取得
        if (particle1 == null || particle2 == null)
        {
            var particles = GetComponentsInChildren<ParticleSystem>();

            if (particles.Length >= 2)
            {
                particle1 = particles[0];
                particle2 = particles[1];
            }
            else if (particles.Length == 1)
            {
                particle1 = particles[0];
            }

            Debug.Log("子オブジェクトのParticleSystemを自動取得しました。");
        }
    }

    void Update()
    {
        // 右クリックでトリガー
        if (Input.GetMouseButtonDown(1))
        {
            isPlaying = !isPlaying;

            if (isPlaying)
            {
                StartParticles();
            }
            else
            {
                StopParticles();
            }
        }
    }

    void StartParticles()
    {
        if (particle1 != null && !particle1.isPlaying)
            particle1.Play();

        if (particle2 != null && !particle2.isPlaying)
            particle2.Play();

        Debug.Log("パーティクル再生開始");
    }

    void StopParticles()
    {
        if (particle1 != null && particle1.isPlaying)
            particle1.Stop();

        if (particle2 != null && particle2.isPlaying)
            particle2.Stop();

        Debug.Log("パーティクル停止");
    }
}
