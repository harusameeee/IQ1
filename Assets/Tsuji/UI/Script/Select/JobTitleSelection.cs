using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

//職業選択用のオブジェクトにつけるやつ
public class JobTitleSelection : MonoBehaviour
{
    //職業のモチーフ画像　オブジェクトの方がいいか？
    [SerializeField] Image jobImage;


    private void Start()
    {
        ImageRotate();
    }

    void Update()
    {
        //画面外なら処理止めるとかあったよな
    }

    // 職業オブジェクトに触れたら
    private void OnTriggerEnter(Collider other)
    {
        //既にその職業ならスキップ
        //if () { return; }
        //playerなら職を与える
        //other.gameObject.AddComponent
    }

    private void ImageRotate()
    {
        //回転軸*度数,秒数　なんか変
        //jobImage.transform.DORotate(Vector3.up * 90f, 3f).SetLoops(-1, LoopType.Yoyo);

        //ふよふよ
        jobImage.transform.DOMoveY(1f, 2f).SetRelative(true).SetLoops(-1,LoopType.Yoyo);
    }
}
