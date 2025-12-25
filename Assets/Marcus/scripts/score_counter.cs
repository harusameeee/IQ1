using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class score_counter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static float currentscore = 0;
    int combo = 0;
    public TMPro.TMP_Text comboText;

    [SerializeField] private float floatDistance = 30f;
    [SerializeField] private float duration = 2.0f;

    [SerializeField]private Vector3 defaultPosition;

    [SerializeField] private ScoreData scoreData;

    [Header("Image明るさ設定")]
    [SerializeField] private Image targetImage;   
    [SerializeField] private float normalAlpha = 0.3f;   // 通常の薄い状態
    [SerializeField] private float brightAlpha = 1.0f;  // コンボ時の明るい状態
    [SerializeField] private float flashTime = 0.2f;    // 明るくなる時間


    void Start()
    {
        currentscore = 0;
        entity.onHit += addscore;
        itempickup.scorechange += addscore;
        comboText.text = combo.ToString();

    }

    public void addscore(float scoretoadd,bool comboable)
    {

        if (scoretoadd > 0)
        {
            
            currentscore += scoretoadd * (1f + combo * 0.1f);
            if (comboable)
            {    
                combo += 1;
                FlashImage();
            }
        }
        else
        {
            combo = 0;
            currentscore += scoretoadd;
        }
        scoreData.score = (int)currentscore;
        comboText.text =  combo+ "\nCombo ";
    }

    private void FlashImage()
    {
        if (targetImage == null) return;

        targetImage.DOKill();
        comboText.DOKill();

        // αを1（明るい）に
        targetImage.DOFade(brightAlpha, flashTime)
            .OnComplete(() =>
            {
                // 元の薄い α に戻す
                targetImage.DOFade(normalAlpha, flashTime);
            });
        comboText.DOFade(brightAlpha, flashTime)
            .OnComplete(() =>
            {
                // 元の薄い α に戻す
                comboText.DOFade(normalAlpha, flashTime);
            });
    }

    private void OnDestroy()
    {
        combo = 0;
        currentscore = 0;
    }

    private void OnDisable()
    {
        entity.onHit -= addscore;
        itempickup.scorechange -= addscore;
    }
}
