using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] Sprite[] images;
    [SerializeField] Image[] imageObjects;

    public bool isActive { get; set; } = false;

    private void Start()
    {
        // ループ処理を開始
        LoopActive().Forget();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isActive = true;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            isActive = false;
        }
    }

    /// <summary>
    /// isActiveのON/OFFをずっと監視するループ
    /// </summary>
    private async UniTaskVoid LoopActive()
    {
        while (true)
        {
            // ONになるのを待つ
            await UniTask.WaitUntil(() => isActive);
            RandomImage();
            Activation();

            // OFFになるのを待つ
            await UniTask.WaitUntil(() => !isActive);
            Termination();
        }
    }

    private void RandomImage()
    {
        int length = imageObjects.Length;
        for (int i = 0; i < length; i++)
        {
            int reverse = i >= length / 2 ? -1 : 1;
            int rand = Random.Range(0, images.Length);

            // Spriteを設定（Image用）
            imageObjects[i].sprite = images[rand];

            // 反転処理
            Vector3 scale = imageObjects[i].transform.localScale;
            scale.x = Mathf.Abs(scale.x) * reverse;
            imageObjects[i].transform.localScale = scale;
        }
    }

    private void Activation()
    {
        int length = imageObjects.Length;
        for (int i = 0; i < length; i++)
        {
            int reverse = i >= length / 2 ? 1 : -1;
            float rand = Random.Range(0.5f, 1.5f);

            imageObjects[i].transform
                .DOLocalMoveX(250f * reverse, 0.5f * rand)
                .SetEase(Ease.OutBack);
        }
    }

    private void Termination()
    {
        int length = imageObjects.Length;
        for (int i = 0; i < length; i++)
        {
            int reverse = i >= length / 2 ? -1 : 1;
            float rand = Random.Range(0.5f, 1.5f);

            imageObjects[i].transform
                .DOLocalMoveX(1800f * reverse, 2f * rand)
                .SetEase(Ease.InBack);
        }
    }
}
