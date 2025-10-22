using System.Collections.Generic;
using UnityEngine;

public abstract class entity : hurtbox,Damagable
{
    public List<buffdata> buffs = new List<buffdata>();
    public virtual bool TakeDamage(int damageAmount)
    {
        return true;
    }
    public bool TakeDamage_screenaoe(int damageAmount, hurtbox hb, hurtbox.targetype targettype)
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
    public class buffdata
    {
        public string buffname;
        public bufftypes type;
        public float pow;
        public float duration;
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
    }
}
