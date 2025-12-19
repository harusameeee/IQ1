using UnityEngine;

public class ResultPlayer : MonoBehaviour
{
    //職業プレハブ
    [NamedArray(new string[] { "ninja", "merlion", "tonto" })]
    [SerializeField] private GameObject[] jobPrehub = new GameObject[3];

    //配置位置
    [SerializeField] private Transform[] transforms = new RectTransform[2];

    //生成したプレイヤーの Animator を保存
    private Animator[] playerAnimators = new Animator[2];


    //  プレイヤー生成
    public void JobChange(string jobName, int playerNum)
    {
        GameObject player = null;

        switch (jobName)
        {
            case "ninja":
                player = Instantiate(jobPrehub[0], transforms[playerNum]);
                break;

            case "merlion":
                Vector3 plus=new Vector3(0.0f, 0.5f, 0.0f);
                transforms[playerNum].position += plus;
                player = Instantiate(jobPrehub[1], transforms[playerNum]);
                break;

            case "tonto":
                player = Instantiate(jobPrehub[2], transforms[playerNum]);
                break;
        }

        if (player != null)
        {
            // スケール調整
            player.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            // Animator を取得して保存
            playerAnimators[playerNum] = player.GetComponent<Animator>();
        }
    }


    //  アニメーション再生
    public void PlayerAnim(bool clear, int playerNum)
    {
        Animator anim = playerAnimators[playerNum];
        if (anim == null) return;

        if (clear)
        {
            anim.SetTrigger("Clear");
        }
        else
        {
            anim.SetTrigger("GameOver");
        }
    }
}
