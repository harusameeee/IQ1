using System;
using UnityEngine;

public class hurtbox : MonoBehaviour
{
    //position of the hurtbox in local space (0,0 is the center position on screen)    //ローカル空間におけるヒットボックスの位置（0,0は画面上の中心位置）
    public virtual Vector2 position => new Vector2(-transform.localPosition.x, transform.localPosition.y + 1.5f);
    // the dimension of the hurtbox     // ハートボックスの寸法
    public virtual Vector2 dimension => new Vector2(1.0f, 1.0f);
    
    public static Action<int> onHit;
    
    public hurtbox.targetype targettype;

    public enum targetype
    {
        enemy,
        player,
    }
}
