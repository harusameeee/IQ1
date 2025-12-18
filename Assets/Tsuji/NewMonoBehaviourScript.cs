using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    //生成したプレイヤーの Animator を保存
    [SerializeField] private Animator[] playerAnimators = new Animator[3];


    private void Update()
    {
        PlayerAnim(0);
        PlayerAnim(1);
        PlayerAnim(2);
    }

    //  アニメーション再生
    public void PlayerAnim(int a)
    {
        Animator anim = playerAnimators[a];
        if (anim == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Clear");
        }
        //else
        //{
        //    anim.SetTrigger("GameOver");
        //}
    }
}


