using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class down : MonoBehaviour
{
    //ìÆÇ≠ïbêî
    [SerializeField] float moveTime=1.0f;

    private void Start()
    {
        MoveDown();
    }

    //è„Ç©ÇÁç~Ç¡ÇƒÇ≠ÇÈÇ›ÇΩÇ¢Ç»
    public void MoveDown()
    {
        this.transform.DOLocalMoveY(0.0f, moveTime).SetEase(Ease.OutElastic);
    }
}
