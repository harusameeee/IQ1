using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    [Header("ステージオブジェクト")]
    [NamedArray(new string[] { "タイトルに戻る", "スタート地点", "ステージ1" })]
    [SerializeField] private GameObject[] stageObjects = new GameObject[3];

    [Header("プレイヤー")]
    [SerializeField] private GameObject[] players = new GameObject[2];
    private Transform[] playerTransforms;
    //ステージ記録用
    [SerializeField] private SelectedStage selectedStage;
    //選択中の番号
    int selectNum = 1;
    //動作中か？
    bool isMove = false;
    //ステージ選択したか？
    public bool isSelect { get; set; } = false; 

    // Update is called once per frame
    void Update()
    {
        //A
        if (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Space))
        {
            if (isMove || selectNum == 1) { return; }
            isMove = true;

            if (selectNum == 0)
            {
                //タイトルに戻る
                var a =new SceneChanger();
                a.ToTitle();
            }
            else 
            {
                //ステージを記録
                selectedStage.SetStageName(stageObjects[selectNum].name);
                isSelect= true;
            }
        }

        float horizontal = Input.GetAxis("Horizontal"); 

        //左 left
        if (horizontal < -0.5f||Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //既に左端=0の時
            if (selectNum == 0 || isMove) { return; }
            isMove = true;
            selectNum--;
            MovePlayer(stageObjects[selectNum].transform).Forget();
        }
        //右 right
        else if (horizontal > 0.5f || Input.GetKeyDown(KeyCode.RightArrow))
        {
            //既に右端=配列の長さの時
            if (selectNum == stageObjects.Length-1 || isMove) { return; }
            isMove = true;
            selectNum++;
            MovePlayer(stageObjects[selectNum].transform).Forget();
        }
    }

    private async UniTask MovePlayer(Transform stage)
    {
        Vector3 pos1 = stage.localPosition - new Vector3(1.5f, 0f, 0f);
        Vector3 pos2 = stage.localPosition + new Vector3(1.5f, 0f, 0f);
        Tween tween = players[0].transform
                .DOLocalMove(pos1, 0.5f)
                .SetEase(Ease.OutQuad); 
        Tween tween2 = players[1].transform
                .DOLocalMove(pos2, 0.7f)
                .SetEase(Ease.OutQuad);

        // DOTween の完了を UniTask で待つ
        await UniTask.WaitUntil(() => !tween.IsActive() || tween.IsComplete());
        await UniTask.WaitUntil(() => !tween2.IsActive() || tween2.IsComplete());

        // 移動完了後
        isMove = false;
    }
}
