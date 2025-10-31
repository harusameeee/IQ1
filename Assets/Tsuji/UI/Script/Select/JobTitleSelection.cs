using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

//職業選択用のオブジェクトにつけるやつ
public class JobTitleSelection : MonoBehaviour
{
    //職業のモチーフ画像　オブジェクトの方がいいか？
    [SerializeField] Image jobImage;

    //playerspawner playerspawner;
    string jobName;

    [SerializeField] private SelectedPlayerJob[] playerJob=new SelectedPlayerJob[2];

    private void Start()
    {
        ImageRotate();
        jobName=jobImage.name;
    }

    void Update()
    {
        //画面外なら処理止めるとかあったよな
    }

    // 職業オブジェクトに触れたら
    private void OnTriggerEnter(Collider other)
    {
        string name=other.name;
        int player = name == "Player1" ? 0 : 1;
        //既にその職業ならスキップ
        if (playerJob[0].playerJobName==jobName) { return; }
        //playerなら職を与える(見た目だけかも→別スクリプト必要)
        playerJob[0].playerJobName=jobName;
        Debug.Log(playerJob[0].playerJobName);
   

    }

    private void ImageRotate()
    {
        //回転軸*度数,秒数　なんか変
        //jobImage.transform.DORotate(Vector3.up * 90f, 3f).SetLoops(-1, LoopType.Yoyo);

        //ふよふよ
        jobImage.transform.DOMoveY(1f, 2f).SetRelative(true).SetLoops(-1,LoopType.Yoyo);
    }
}
