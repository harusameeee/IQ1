using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] Sprite[] images = new Sprite[4];  // 0:GO, 1:1, 2:2, 3:3
    [SerializeField] Image imageObject;

    //private float time = 3.0f;

    //いる？
    //public bool finishCount = false;

    private void Start()
    {
        // 演出付きカウントダウン開始
        StartCountdown().Forget();
    }

    private async UniTaskVoid StartCountdown()
    {
        // 3, 2, 1 の表示
        for (int i = 3; i > 0; i--)
        {
            imageObject.sprite = images[i];
            PlayPopAnimation();
            await UniTask.Delay(1000); // 1秒待つ
        }

        // 最後に「GO!」演出
        imageObject.sprite = images[0];
        //finishCount = true;
        PlayPopAnimation();

        // ちょっと待ってからフェードアウト
        await UniTask.Delay(800);
        imageObject.DOFade(0f, 0.5f)
            .SetEase(Ease.InOutQuad)
            // 消せるんかな
            .SetAutoKill(true)
            .SetLink(imageObject.gameObject);
        
    }

    /// <summary>
    /// 拡縮演出
    /// </summary>
    private void PlayPopAnimation()
    {
        imageObject.transform.localScale = Vector3.zero;
        Vector3 size = imageObject.sprite.bounds.size;
        imageObject.transform
            .DOScale(size*2, 0.3f)
            .SetEase(Ease.OutBack); // ポンッと出る感じ
    }

    //

}
