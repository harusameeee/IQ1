using UnityEngine;
using UnityEngine.UI;

public class JobUIAllocation : MonoBehaviour
{
    [NamedArray(new string[] { "attack", "attack2", "skill", "defence", "Icon" })]
    [SerializeField] Image[] playerImage;

    [SerializeField] SelectedPlayerJob playerjob;
    [SerializeField] JobUI[] jobUIs;

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
                targetSprites = jobUIs[0].jobUI;
                break;
            case "tonto":
                targetSprites = jobUIs[1].jobUI;
                break;
            case "marlion":
                targetSprites = jobUIs[2].jobUI;
                break;
        }

        if (targetSprites == null) return;


        for (int i = 0; i < playerImage.Length; i++)
        {
            playerImage[i].sprite = targetSprites[i];
        }
    }
}
