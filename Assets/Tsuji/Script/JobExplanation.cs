using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class JobExplanation : MonoBehaviour
{
    //UIの画像が入っているやつ
    [SerializeField] JobUI[] jobUIs=new JobUI[3];
    //UI置くところ
    [NamedArray(new string[] 
    { "attack", "attack2", "skill", "defence", "flag", "jobIcon","name",
      "setumei_attack","setumei_attack2","setumei_skill","setumei_defence"})]
    [SerializeField] Image[] details=new Image[11];
    bool isAnimating = false;
    [SerializeField] CoinDisplay coinDisplay;
    public void ChangeJobImage(int jobNum)
    {
        switch (jobNum)
        {
            //ninja
            case 0:
                for(int i=0; i<details.Length; i++)
                {
                    details[i].sprite= jobUIs[0].jobUI[i];
                    coinDisplay.CoinDisp(false);
                }
                break;

            //marlion
            case 2:
                for (int i = 0; i < details.Length; i++)
                {
                    details[i].sprite = jobUIs[2].jobUI[i];
                    coinDisplay.CoinDisp(true);
                }
                break;

            //tontto
            case 1:
                for (int i = 0; i < details.Length; i++)
                {
                    details[i].sprite = jobUIs[1].jobUI[i];
                    coinDisplay.CoinDisp(false);
                }
                break;
        }
    }

    public void TurnOverImage(bool undo)
    {
        if (isAnimating) { return; }
        isAnimating = true;
        //元に戻すときは逆回転
        int a = undo ? 1 : -1;
        Vector3 rotate = new (0, 180 * a, 0);
        this.transform.DOLocalRotate(transform.localEulerAngles + rotate, 0.5f).SetEase(Ease.OutQuad)
             .OnComplete(() =>
             {
                 isAnimating = false; // ← アニメ終了で解除
             });
    }

    public string GetJobName(int jobNum)
    {
        string name = "";
        switch (jobNum)
        {
            case 0:
                name = "ninja";
                break;
            case 2:
                name = "merlion";
                break;
            case 1:
                name = "tonto";
                break;
        }
        return name;
    }

}
