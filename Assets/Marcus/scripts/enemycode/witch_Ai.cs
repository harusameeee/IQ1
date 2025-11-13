using System;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;
public class witch_Ai : entity
{
    
    protected static Random rng = new();
    public witch_mover mover;
    //will add spawn patern soon
    public GameObject projectile;
    public float shootInterval = 2.0f;
    private float shootTimer = 0.0f;
    public int maxdist = 5;
    public Vector2 dim;
    public override Vector2 dimension => dim;
    public override Vector2 position => new Vector2(transform.localPosition.x, transform.localPosition.y + 8f);
    public static Action<float,List<damagable_type>,Vector2> enemyhit ;
    public override void Start()
    {
        base.Start();
    }
    public override bool TakeDamage(float damageAmount, bool comboable = true, List<damagable_type> damagable_Types = null, Vector2 hitpoint = new Vector2())
    {

        float dmgmult = 1.0f;
        foreach (var buff in buffs)
        {
            if (buff.type == bufftypes.vulnerability)
            {
                dmgmult += buff.pow;
            }
        }
        Debug.Log($"Witch took {damageAmount} damage with mult {dmgmult}");
        onHit?.Invoke((int)(damageAmount * dmgmult), comboable);
        float xOffset = rng.Next(-5, 5);
        float yOffset = rng.Next(-5, 5);
        hitpoint = new Vector2(hitpoint.x + xOffset/5, hitpoint.y + yOffset/5);
        enemyhit?.Invoke((int)(damageAmount * dmgmult), damagable_Types, hitpoint);
        return true;
    }
    
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        return;
       
    }
}
