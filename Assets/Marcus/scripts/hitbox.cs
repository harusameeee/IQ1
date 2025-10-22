using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
public class hitbox : hurtbox
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform reftransform;
    public override Vector2 position => transform.localPosition-reftransform.localPosition;
    public override Vector2 dimension => transform.localScale;
    public int dmgamount;
    public skilldata skilldata;
    //notes pos x and y are for abs pos
    //scale x and y are for size
    //maybe will add delay timer???
    public entity owner;
    public List<entity> damagables = new List<entity>();
    public List<entity> alr_damaged = new List<entity>();
    void Start()
    {
        var ss = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<entity>();
        foreach (entity s in ss) {
            damagables.Add (s);
        }
    }
    public virtual void FixedUpdate()
    {
        foreach (entity d in damagables.Where(d => !alr_damaged.Contains(d)))
        {
            if (d.TakeDamage_screenaoe(dmgamount, this, targettype))
            {
                float dmgmult = 1.0f;
                for (int i =0; i< owner.buffs.Count;i++)
                {
                    var buff = owner.buffs[i];
                    if (buff.type == entity.bufftypes.attack)
                    {
                        dmgmult += buff.pow;
                        Debug.Log($"P{owner.name} attack buff applied: {buff.pow}");
                    }
                    else if (buff.type == entity.bufftypes.stealth)
                    {
                        dmgmult += buff.pow;
                        Debug.Log($"P{owner.name} stealth buff applied: {buff.pow}");
                        owner.removebuff(i);
                    }
                }
                d.TakeDamage((int)(dmgamount * dmgmult));
                foreach (var effect in skilldata.effects)
                {
                    effect.activeeffect(owner, d);
                }
                alr_damaged.Add(d);
            }
        }
    }
    void OnDisable()
    {
        alr_damaged.Clear();//clear damagables on disable
        skilldata = null;
    }
    void OnEnable()
    {
        alr_damaged.Clear();//clear damagables on enable
    }
}
