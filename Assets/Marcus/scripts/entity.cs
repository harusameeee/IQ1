using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class entity : hurtbox, Damagable
{
    [Header("buffs")]
    [SerializeReference]
    public List<buffdata> buffs = new List<buffdata>();
    public bool showbufficons = false;
    public List<bufficon> bufficons = new List<bufficon>();
    public Transform bufficonparent;
    [Header("entity events")]
    public static Action<float, bool> onHit;
    [HideInInspector] public float elapsedtime = 0f;
    public static Action<entity> onspawn;
    public SkinnedMeshRenderer rend;
    public virtual void Start()
    {
        if (showbufficons)
        {
            for (int i = 0; i < 5; i++)
            {
                bufficon temp = Instantiate(Resources.Load<bufficon>("buff_icon"), bufficonparent);
                bufficons.Add(temp);
                temp.gameObject.SetActive(false);
            }
        }
        onspawn?.Invoke(this);
    }
    public virtual void Update()
    {
        elapsedtime += Time.deltaTime;
        if (elapsedtime >= 1f)
        {
            elapsedtime = elapsedtime % 1f;
            countdownbuffdurations();
        }
    }

    public virtual bool TakeDamage(float damageAmount, bool comboable, List<damagable_type> damagable_Types = null, Vector2 hitpoint = new Vector2())
    {
        return true;
    }
    public virtual bool TakeDamage_screenaoe(float damageAmount, hurtbox hb, hurtbox.targetype targettype, out Rect overlap, List<damagable_type> damagable_Types = null)
    {
        if (targettype != this.targettype)
        {
            overlap = new Rect();
            return false;
        }
        if (hb.is_circle)
        {
            // Circle-rectangle collision detection
            Vector2 circleCenter = hb.position;
            float radius = hb.dimension.x / 2; // Assuming dimension.x is diameter

            Rect rect = new Rect(position - dimension / 2, dimension);

            // Find the closest point to the circle within the rectangle
            float closestX = Mathf.Clamp(circleCenter.x, rect.xMin, rect.xMax);
            float closestY = Mathf.Clamp(circleCenter.y, rect.yMin, rect.yMax);

            // Calculate the distance between the circle's center and this closest point
            float distanceX = circleCenter.x - closestX;
            float distanceY = circleCenter.y - closestY;

            // If the distance is less than the circle's radius, an intersection occurs
            float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
            if (distanceSquared < (radius * radius))
            {
                // Calculate overlap rectangle
                float overlapXMin = Mathf.Max(rect.xMin, circleCenter.x - radius);
                float overlapYMin = Mathf.Max(rect.yMin, circleCenter.y - radius);
                float overlapXMax = Mathf.Min(rect.xMax, circleCenter.x + radius);
                float overlapYMax = Mathf.Min(rect.yMax, circleCenter.y + radius);
                overlap = new Rect(overlapXMin, overlapYMin, overlapXMax - overlapXMin, overlapYMax - overlapYMin);
                return true;
            }
            overlap = new Rect();
            return false;
        }
        Rect self = new Rect(position - dimension / 2, dimension);
        Rect other = new Rect(hb.position - hb.dimension / 2, hb.dimension);
        overlap = GetOverlapRect(self, other, out bool isOverlapping);
        if (isOverlapping)
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
    public virtual void addbuff(buffdata newBuff)
    {
        var existingBuff = buffs.Find(x => x.buffname == newBuff.buffname);
        if (existingBuff != null)
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

            PlayBuffVFX(buffToAdd.type);
        }

    }
    public void countdownbuffdurations()
    {

        for (int i = buffs.Count - 1; i >= 0; i--)
        {

            buffs[i].duration--;
            if (buffs[i].duration <= 0)
            {
                removebuff(i);
            }
            else if (buffs[i].type == bufftypes.poison)
            {
                TakeDamage(buffs[i].pow, false, new List<damagable_type>() { damagable_type.poison }, Vector2.zero);
            }
        }
    }
    Rect GetOverlapRect(Rect a, Rect b, out bool isOverlapping)
    {
        isOverlapping = false;
        float xMin = Mathf.Max(a.xMin, b.xMin);
        float yMin = Mathf.Max(a.yMin, b.yMin);
        float xMax = Mathf.Min(a.xMax, b.xMax);
        float yMax = Mathf.Min(a.yMax, b.yMax);

        // Check if they actually overlap
        if (xMax <= xMin || yMax <= yMin)
            return new Rect(); // Empty rect (no overlap)

        isOverlapping = true;
        return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
    }

    public IEnumerator dmgflash()
    {
        int flashlength = 5;
        foreach (var mat in rend.materials)
        {
            mat.SetColor("_Emissive_Color", Color.white);
        }
        while (flashlength > 0)
        {
            flashlength--;
            yield return null;
        }
        foreach (var mat in rend.materials)
        {
            mat.SetColor("_Emissive_Color", Color.black);
        }

    }

    public virtual void PlayBuffVFX(bufftypes type)
    {
        GameObject fxPrefab = null;

        switch (type)
        {
            case bufftypes.speed_increase:
                fxPrefab = Resources.Load<GameObject>("VFX/SpeedBuff_VFX");
                break;
            case bufftypes.attack:
                fxPrefab = Resources.Load<GameObject>("VFX/AttackBuff_VFX");
                break;
        }

        if (fxPrefab != null)
        {
            GameObject fx = Instantiate(fxPrefab, transform.position, Quaternion.identity);
            fx.transform.SetParent(transform, worldPositionStays: true);

            //  àÍíËéûä‘å„Ç…è¡Ç∑ÅiVFXÇ™é©ìÆÇ≈èIÇÌÇÁÇ»Ç¢èÍçáÅj
            //Destroy(fx, 3f);
        }
    }

}
