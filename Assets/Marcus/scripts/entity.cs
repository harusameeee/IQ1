using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class entity : hurtbox,Damagable
{
    public List<buffdata> buffs = new List<buffdata>();


    public static Action<float,bool> onHit;
    public float elapsedtime = 0f;
    public virtual void Update()
    {
        elapsedtime += Time.deltaTime;
        if (elapsedtime >= 1f) {
            elapsedtime = elapsedtime % 1f; 
            countdownbuffdurations();
        }
    }

    public virtual bool TakeDamage(float damageAmount,bool comboable)
    {
        return true;
    }
    public bool TakeDamage_screenaoe(float damageAmount, hurtbox hb, hurtbox.targetype targettype)
    {
        if (targettype != this.targettype)
        {
            return false;
        }
        Rect self = new Rect(position - dimension / 2, dimension);
        Rect other = new Rect(hb.position - hb.dimension / 2, hb.dimension);
        if (self.Overlaps(other))
        {
            return true;
        }
        return false;
    }
    public void removebuff(int index)
    {
        buffs.RemoveAt(index);
    }
   public void countdownbuffdurations()
    {

        for(int i = buffs.Count -1; i >=0; i--)
        {
            
            buffs[i].duration --;
            if (buffs[i].duration <= 0)
            {
                buffs.RemoveAt(i);
            }
            else if(buffs[i].type == bufftypes.poison)
            {
                TakeDamage(buffs[i].pow,false);
            }
        }
    }
    [Serializable]
    public class buffdata
    {
        public bool stackable;
        public string buffname;
        public bufftypes type;
        public float pow;
        public float duration;
        public buffdata copy()
        {
            buffdata newbuff = new buffdata();
            newbuff.stackable = stackable;
            newbuff.buffname = buffname;
            newbuff.type = type;
            newbuff.pow = pow;
            newbuff.duration = duration;
            return newbuff;
        }
    }
    public enum bufftypes
    {
        gcd_reduction,
        cooldown_reduction,
        speed_increase,//speed increase buff
        attack,//attack buff
        vulnerability,//takes more damage
        stealth,//invuln+ damage boost when hitting from stealth
        invuln///completely invulnerable
        ,poison
    }
    
}
