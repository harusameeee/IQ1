using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class score_counter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static float currentscore = 0;
    int combo = 0;
    //public TMPro.TMP_Text scoretext;
    public TMPro.TMP_Text comboText;

    [SerializeField] private float floatDistance = 50f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Color startColor = Color.white;

    [SerializeField]private Vector3 defaultPosition;

    [SerializeField] private ScoreData scoreData;

    void Start()
    {
        entity.onHit += addscore;
        itempickup.scorechange += addscore;
        currentscore = 0;
        //scoretext.text = "Score: " + ((int)currentscore);
        comboText.text = combo + "\nCOMBO ";

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            addscore(1,true);
        }
    }
    public void addscore(float scoretoadd,bool comboable)
    {
        //出来てるか確認
        if (scoretoadd > 0)
        {
            
            currentscore += scoretoadd * (1f + combo * 0.1f);
            if (comboable)
            {    
            combo += 1;
            ComboAction().Forget();
            }
        }
        else
        {
            combo = 0;
            currentscore += scoretoadd;
            scoreData.score = (int)currentscore;

        }
        
           // scoretext.text = "Score: " +  ((int)currentscore);
            comboText.text =  combo+ "\nCombo ";
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
}
