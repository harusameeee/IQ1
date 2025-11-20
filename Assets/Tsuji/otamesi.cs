using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class otamesi : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static float currentscore = 0;
    int combo = 0;
    //public TMPro.TMP_Text scoretext;
    public TMPro.TMP_Text comboText;

    [SerializeField] private float floatDistance = 30f;
    [SerializeField] private float duration = 2.0f;
    [SerializeField] private Color startColor = Color.white;

    [SerializeField] private Vector3 defaultPosition;

    [SerializeField] private ScoreData scoreData;


    [Header("Image明るさ設定")]
    [SerializeField] private Image targetImage;   // ← Image を入れる
    [SerializeField] private float normalAlpha = 0.3f;   // 通常の薄い状態
    [SerializeField] private float brightAlpha = 1.0f;  // コンボ時の明るい状態
    [SerializeField] private float flashTime = 0.2f;    // 明るくなる時間

    void Start()
    {
        //entity.onHit += addscore;
        //itempickup.scorechange += addscore;
        currentscore = 0;
        //scoretext.text = "Score: " + ((int)currentscore);
        

        if (targetImage != null)
        {
            var c = targetImage.color;
            c.a = normalAlpha;
            targetImage.color = c;  // 初期状態を薄く
        }
        if (comboText != null)
        {
            comboText.text = combo.ToString();
            var c = comboText.color;
            c.a = normalAlpha;
            comboText.color = c;  // 初期状態を薄く
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            addscore(1, true);
        }
    }
    public void addscore(float scoretoadd, bool comboable)
    {

        if (scoretoadd > 0)
        {

            currentscore += scoretoadd * (1f + combo * 0.1f);
            if (comboable)
            {
                combo += 1;
                //ComboAction().Forget();
                FlashImage();
            }
        }
        else
        {
            combo = 0;
            currentscore += scoretoadd;
            scoreData.score = (int)currentscore;

        }

        // scoretext.text = "Score: " +  ((int)currentscore);
        comboText.text = combo.ToString() ;
    }

    //コンボ表示イベント
    private async UniTask ComboAction()
    {
        comboText.DOKill();
        comboText.rectTransform.localPosition = defaultPosition;
        comboText.color = new Color(startColor.r, startColor.g, startColor.b, 1f);

        var moveTween = comboText.rectTransform.DOLocalMoveY(defaultPosition.y + floatDistance, duration)
            .SetEase(Ease.OutQuad);
        var fadeTween = comboText.DOFade(0f, duration);

        await DOTween.Sequence()
            .Append(moveTween)
            .Join(fadeTween)
            .SetEase(Ease.OutQuad)
            .AsyncWaitForCompletion();

        comboText.rectTransform.localPosition = defaultPosition;
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

}


