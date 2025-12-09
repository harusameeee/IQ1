using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class JobExplanation : MonoBehaviour
{
    //UI‚Ì‰æ‘œ‚ª“ü‚Á‚Ä‚¢‚é‚â‚Â
    [SerializeField] JobUI jobUI;
    //UI’u‚­‚Æ‚±‚ë
    [NamedArray(new string[] { "attack", "attack2", "skill", "defence","flag","jobIcon","name" })]
    [SerializeField] Image[] details=new Image[7];

    public void ChangeJobImage(int jobNum)
    {
        switch (jobNum)
        {
            //ninja
            case 0:
                for(int i=0; i<details.Length; i++)
                {
                    details[i].sprite=jobUI.ninja[i];
                }
                break;

            //marlion
            case 1:
                for (int i = 0; i < details.Length; i++)
                {
                    details[i].sprite = jobUI.marlion[i];
                }
                break;

            //tontto
            case 2:
                for (int i = 0; i < details.Length; i++)
                {
                    details[i].sprite = jobUI.tonto[i];
                }
                break;
        }
    }

    public void TurnOverImage(bool undo)
    {
        //Œ³‚É–ß‚·‚Æ‚«‚Í‹t‰ñ“]
        int a = undo ? 1 : -1;
        Vector3 rotate = new (0, 180 * a, 0);
        this.transform.DOLocalRotate(transform.localEulerAngles + rotate, 0.5f).SetEase(Ease.OutQuad);
    }

    public string GetJobName(int jobNum)
    {
        string name = "";
        switch (jobNum)
        {
            case 0:
                name = "ninja";
                break;
            case 1:
                name = "marlion";
                break;
            case 2:
                name = "tonto";
                break;
        }
        return name;
    }

}
