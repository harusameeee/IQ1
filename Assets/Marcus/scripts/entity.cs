using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class entity : hurtbox,Damagable
{
    [Header("buffs")]
    public List<buffdata> buffs = new List<buffdata>();
    public bool showbufficons = false;
    public List<bufficon> bufficons = new List<bufficon>();
    public Transform bufficonparent;
    [Header("entity events")]
    public static Action<float,bool> onHit;
    [HideInInspector]public float elapsedtime = 0f;
    public virtual void Start()
    {
        if (showbufficons)
        {     for (int i =0;i<5;i++)
        {
        bufficon temp =Instantiate(Resources.Load<bufficon>("buff_icon"),bufficonparent);
        bufficons.Add(temp);
        temp.gameObject.SetActive(false);
        }
        }
    }
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
        if (showbufficons)
            bufficons.Find(b => b.referencedbuff.buffname == buffs[index].buffname)?.gameObject.SetActive(false);
        buffs.RemoveAt(index);
    }
    public void addbuff(buffdata newBuff)
    {        
        var existingBuff = buffs.Find(x => x.buffname == newBuff.buffname);
        if (existingBuff != null )
        {
            if (!newBuff.stackable) return;
            existingBuff.pow += newBuff.pow;
            existingBuff.duration = Mathf.Max(existingBuff.duration, newBuff.duration);
        }
        else
        {
            buffs.Add(newBuff.copy());
            if (showbufficons)
            {
                var icon = bufficons.Find(b => !b.gameObject.activeSelf);
                if (icon != null)
                {
                    icon.referencedbuff = newBuff;
                    icon.buffimg.sprite = newBuff.icon;
                    icon.transform.SetAsLastSibling();
                    icon.gameObject.SetActive(true);
                }
            }
        }
        
    }
   public void countdownbuffdurations()
    {

        for(int i = buffs.Count -1; i >=0; i--)
        {
            
            buffs[i].duration --;
            if (buffs[i].duration <= 0)
            {
               removebuff(i);
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
        public Sprite icon;
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
