using UnityEngine;
using UnityEngine.UI;

public class JobUIAllocation : MonoBehaviour
{
    [NamedArray(new string[] { "attack", "attack2", "skill", "defence", "Icon" })]
    [SerializeField] Image[] playerImage;

    [SerializeField] SelectedPlayerJob playerjob;
    [SerializeField] JobUI jobUI;

    private string lastJobName;

    void Start()
    {
        //Debug.Log(playerjob.name);
        UIChange(); // èâä˙âªéûÇ…Ç‡îΩâf
        lastJobName = playerjob.playerJobName;
    }

    void UIChange()
    {
        Sprite[] targetSprites = null;

        switch (playerjob.playerJobName)
        {
            case "ninja":
                targetSprites = jobUI.ninja;
                break;
            case "tonto":
                targetSprites = jobUI.tonto;
                break;
            case "marlion":
                targetSprites = jobUI.marlion;
                break;
        }

        if (targetSprites == null) return;


        for (int i = 0; i < playerImage.Length; i++)
        {
            playerImage[i].sprite = targetSprites[i];
        }
    }
}
