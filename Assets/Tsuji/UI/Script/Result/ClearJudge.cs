using UnityEngine;
using UnityEngine.UI;

public class ClearJudge : MonoBehaviour
{
    private Image image;
    [SerializeField] private Sprite[] clearSprites=new Sprite[2];

    bool isClear=true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //‘¼ƒNƒ‰ƒX‚©‚ç‚Á‚Ä‚­‚éA‘½•ª
        bool judge = isClear /*!= null ? true : false*/;

        //
        image = gameObject.GetComponent<Image>();

        CheckClear(judge);
    }

    private void CheckClear(bool judge)
    {
        if (!judge)
        {
            //gameOver
            image.sprite = clearSprites[0];
        }
        else
        {
            //clear
            image.sprite = clearSprites[1];
        }
    } 
}
