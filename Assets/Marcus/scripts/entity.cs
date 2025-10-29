using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class entity : hurtbox,Damagable
{
    [Header("buffs")]
    [SerializeReference]
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

    public virtual bool TakeDamage(float damageAmount,bool comboable,List<damagable_type> damagable_Types = null)
    {
        return true;
    }
    public bool TakeDamage_screenaoe(float damageAmount, hurtbox hb, hurtbox.targetype targettype,List<damagable_type> damagable_Types = null)
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
            buffdata buffToAdd = newBuff.copy();
            buffs.Add(buffToAdd);
            if (showbufficons)
            {
                var icon = bufficons.Find(b => !b.gameObject.activeSelf);
                if (icon != null)
                {
                    
                    icon.gameObject.SetActive(true);
                    icon.referencedbuff = buffToAdd;
                    icon.buffimg.sprite = buffToAdd.icon;
                    icon.transform.SetAsLastSibling();
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


    
}
