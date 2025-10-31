using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "detonate_poison", menuName = "ScriptableObjects/Skilleffects/detonate_poison", order = 4)]
public class detonate_poison : skilleffect
{
    public float dmg_mult=1.5f;
    public override void activeeffect(entity user, entity target, skilldata skilldata = null)
    {
        float total_poison = 0f;

        for (int i = target.buffs.Count - 1; i >= 0; i--)
        {
            if (target.buffs[i].type == bufftypes.poison)
            {
                total_poison += target.buffs[i].pow * target.buffs[i].duration;
                target.removebuff(i);
            }
        }
        target.TakeDamage(total_poison * dmg_mult, false, new List<damagable_type>() { damagable_type.poison_deluge });
    }
}
