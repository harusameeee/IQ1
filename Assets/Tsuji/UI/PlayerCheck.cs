using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCheck : MonoBehaviour
{
    //　準備OK
    [SerializeField] Image image;
    //
    StageSelect stageSelect;
    //
    private bool isReady = false;

    SceneChanger changer;

    private void Start()
    {
        transform.localScale = Vector3.zero;
        image.transform.localScale = Vector3.zero;
        stageSelect = FindObjectOfType<StageSelect>();

        // 処理スタート
        CheckSelect().Forget();
        CheckReady().Forget();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && stageSelect.isSelect)
        {
            isReady = true;
        }
    }

    private async UniTask CheckSelect()
    {
        await UniTask.WaitUntil(() => stageSelect.isSelect);
        gameObject.transform.DOScale(Vector3.one, 0.2f);
        
    }

    private async UniTask CheckReady()
    {
        await UniTask.WaitUntil(() => isReady);
        image.transform.DOScale(Vector3.one, 0.2f);
        await UniTask.WaitForSeconds(3);
        transform.DOScale(Vector3.zero, 0.2f);
        changer = new SceneChanger();
        changer.ToPlay();
    }


}
