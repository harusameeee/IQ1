using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
public class hitbox : hurtbox
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform reftransform;
    public override Vector2 position => getpos();
    public override Vector2 dimension => transform.localScale;
    public int dmgamount;
    public List<skilleffect> effects;
    public bool flipx=false;
    //notes pos x and y are for abs pos
    //scale x and y are for size
    //maybe will add delay timer???
    public entity owner;
    public List<entity> alr_damaged = new List<entity>();
    void Awake()
    {
        
    }
    void Start()
    {
    }
    public virtual void FixedUpdate()
    {
        foreach (entity d in obstacle_spawner.damagables.Where(d => !alr_damaged.Contains(d)))
        {
            if (d.TakeDamage_screenaoe(dmgamount, this, targettype, out Rect overlap))
            {
                float dmgmult = 1.0f;
                if (owner != null)
                {
                    for (int i = 0; i < owner.buffs.Count; i++)
                    {
                        var buff = owner.buffs[i];
                        if (buff.type == bufftypes.attack)
                        {
                            dmgmult += buff.pow;
                            //Debug.Log($"P{owner.name} attack buff applied: {buff.pow}");
                        }
                        else if (buff.type == bufftypes.stealth)
                        {
                            dmgmult += buff.pow;
                            //Debug.Log($"P{owner.name} stealth buff applied: {buff.pow}");
                            owner.removebuff(i);
                        }
                    }
                }
                //Debug.Log($"{name} dealing {(int)(dmgamount * dmgmult)} damage to {d.name}");
                d.TakeDamage((int)(dmgamount * dmgmult), true, null,new Vector2( overlap.center.x*1.5f+2, overlap.center.y-8f));
                if (effects != null && effects.Count > 0) {
                    foreach (var effect in effects)
                    {
                        effect.activeeffect(owner, d);
                    }
                }

                alr_damaged.Add(d);

            }
        }
    }
    public Vector2 getpos()
    {
        Vector3 temp = transform.localPosition;
        if (reftransform == null)
        {
            return transform.localPosition;
        }
        if (flipx)
        {
            return transform.localPosition + new Vector3(-reftransform.localPosition.x, reftransform.localPosition.y, reftransform.localPosition.z);
        }
        else
        {
            return transform.localPosition + reftransform.localPosition;
        }
            
    }
    void OnDisable()
    {
        alr_damaged.Clear();//clear damagables on disable
        effects = null;
    }
    void OnEnable()
    {
        alr_damaged.Clear();//clear damagables on enable
    }
}
