using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class score_counter : MonoBehaviour
{
    public static float currentscore = 0f;

    private int combo = 0;
    public TMPro.TMP_Text comboText;

    [SerializeField] private ScoreData scoreData;

    [Header("Image明るさ設定")]
    [SerializeField] private Image targetImage;
    [SerializeField] private float normalAlpha = 0.3f;
    [SerializeField] private float brightAlpha = 1.0f;
    [SerializeField] private float flashTime = 0.2f;

    void Start()
    {
        
        entity.onHit += addscore;
        itempickup.scorechange += addscore;

        currentscore = 0;
        combo = 0;
        UpdateUI();
    }

    // ★解除を追加（重要）
    void OnDestroy()
    {
        entity.onHit -= addscore;
        itempickup.scorechange -= addscore;
    }

    public void addscore(float scoretoadd, bool comboable)
    {
        if (scoretoadd > 0)
        {
            if (comboable)
            {
                combo++;
               
                FlashImage();
            }

            currentscore += scoretoadd * (1f + combo * 0.1f);
        }
        else
        {
            combo = 0;
            currentscore += scoretoadd;
        }

        scoreData.score = Mathf.FloorToInt(currentscore);
        UpdateUI();
    }

    private void UpdateUI()
    {
        comboText.text = $"{combo}\nCombo ";
    }

    private void FlashImage()
    {
        if (targetImage == null) return;

        targetImage.DOKill();
        comboText.DOKill();

        targetImage
            .DOFade(brightAlpha, flashTime)
            .OnComplete(() =>
                targetImage.DOFade(normalAlpha, flashTime)
            );

        comboText
            .DOFade(brightAlpha, flashTime)
            .OnComplete(() =>
                comboText.DOFade(normalAlpha, flashTime)
            );
    }
}
