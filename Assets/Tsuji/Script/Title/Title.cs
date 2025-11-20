using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    private bool moved = false;

    // Update is called once per frame
    void Update()
    {
        if(!moved)
        {
            this.transform.DOLocalMoveY(0.0f, 1.0f).SetEase(Ease.OutElastic);
            moved = true;
        }
    }

    private void OnDestroy()
    {
        SoundManager.Instance.StopBGM().Forget();
    }
   
}
