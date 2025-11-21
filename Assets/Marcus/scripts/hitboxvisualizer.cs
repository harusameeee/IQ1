using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
//･ﾒ･ﾃ･ﾈ･ﾜ･ﾃ･ｯ･ｹｲﾄｻ・ｽ･ﾄ｡ｼ･・
public class hitboxvisualizer : MonoBehaviour
{
    public List<hitboxpair> additionalhitboxes = new List<hitboxpair>();//･・ｹ･ﾈ､ﾋ･ﾏ｡ｼ･ﾈ､ﾎ･ﾜ･ﾃ･ｯ･ｹ､ﾉｲﾃ､ｷ､ﾆﾉﾁｲ隍ｹ､・ﾀ､ｱ

    void OnDrawGizmos()
    {
     
        foreach (hitboxpair hbp in additionalhitboxes)
        {
            if (hbp.todraw.active && hbp.todraw.gameObject.activeInHierarchy)
            {
                if (!hbp.todraw.is_circle)
                {

                    drawhurbox(hbp.todraw, hbp.hbcolor);
                }
                else
                {
                    drawcirclehurbox(hbp.todraw, hbp.hbcolor);
                }
            }
        }
    }
    public void drawhurbox(hurtbox hb, Color col)//､ｳ､ﾎｴﾘｿﾈﾍﾑ､ｷ､ﾆ｡｢･ｮ･ｺ･筅ﾋ･ﾒ･ﾃ･ﾈ･ﾜ･ﾃ･ｯ･ｹ､ﾁｲ隍ｷ､ﾞ､ｹ
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.parent.position, transform.parent.rotation * quaternion.Euler(0, math.PI, 0), transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.color = col;
        Gizmos.DrawWireCube(new Vector3(hb.position.x, hb.position.y, 0), new Vector3(hb.dimension.x, hb.dimension.y, 1.0f));
    }
    public void drawcirclehurbox(hurtbox hb, Color col)//､ｳ､ﾎｴﾘｿﾈﾍﾑ､ｷ､ﾆ｡｢･ｮ･ｺ･筅ﾋ･ﾒ･ﾃ･ﾈ･ﾜ･ﾃ･ｯ･ｹ､ﾁｲ隍ｷ､ﾞ､ｹ
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.parent.position, transform.parent.rotation * quaternion.Euler(0, math.PI, 0), transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.color = col;
        Gizmos.DrawWireSphere(new Vector3(hb.position.x, hb.position.y, 0), hb.dimension.x/2);
    }
    [Serializable]
    public class hitboxpair
    {
        public hurtbox todraw;
        public Color hbcolor;
    }
}
