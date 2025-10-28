using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
//ヒットボックス可視化ツール
public class hitboxvisualizer : MonoBehaviour
{
    public List<hitboxpair> additionalhitboxes = new List<hitboxpair>();//リストにハートのボックスを追加して描画するだけ

    void OnDrawGizmos()
    {
     
        foreach (hitboxpair hbp in additionalhitboxes)
        {
            if (hbp.todraw.active&&hbp.todraw.gameObject.activeInHierarchy)
            drawhurbox(hbp.todraw, hbp.hbcolor);
        }
    }
    public void drawhurbox(hurtbox hb, Color col)//この関数を使用して、ギズモにヒットボックスを描画します
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.parent.position, transform.parent.rotation*quaternion.Euler(0, math.PI, 0), transform.lossyScale);
        Gizmos.matrix = rotationMatrix;	
        Gizmos.color = col;
        Gizmos.DrawWireCube(new Vector3(hb.position.x, hb.position.y, 0), new Vector3(hb.dimension.x, hb.dimension.y, 1.0f));
    }
    [Serializable]
    public class hitboxpair
    {
        public hurtbox todraw;
        public Color hbcolor;
    }
}
