using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ObstacleUI : MonoBehaviour
{
    private float y = 0.0f;

    private void Start()
    {
        y = this.transform.position.y;
        MoveObstacle().Forget();
    }

    private async UniTask MoveObstacle()
    {
        float n = Random.Range(70, 100);

        //Ç‰ÇÁÇ‰ÇÁ
        //âÒì]
        this.gameObject.transform.DORotate((Vector3.back*Time.deltaTime*n), 1f).SetLoops(-1, LoopType.Yoyo);
        //è„â∫à⁄ìÆ
        this.gameObject.transform.DOMoveY(y+n, 2f).SetLoops(-1, LoopType.Yoyo);

    }
}
