using System;
using UnityEngine;

public class itempickup : skilleffect
{
    public itemtypes itemtype;
    public float pow = 0;
    public buffdata speedbuff;

    public buffdata dmgbuff;
    public buffdata vulnbuff;
    public buffdata invulnbuff;
    public buffdata cdrbuff;
    public static Action<float, bool> scorechange;
    public override void activeeffect(entity user, entity target, skilldata skilldata = null)
    {
        if (target is PlayerLineMove)
        {
            PlayerLineMove p = target as PlayerLineMove;
            switch (itemtype)
            {
                case itemtypes.Hp_heal:
                    p.heal(pow);
                    break;
                case itemtypes.Score_bonus:
                    scorechange?.Invoke(pow, false);
                    // Implement score bonus logic
                    break;
                case itemtypes.Speed_boost:
                    p.addbuff(speedbuff);
                    break;
                case itemtypes.Damage_multiplier:
                    p.addbuff(dmgbuff);
                    break;
                case itemtypes.Invincibility:
                    p.addbuff(invulnbuff);
                    break;
                case itemtypes.Cooldown_reduction:
                    p.addbuff(cdrbuff);
                    break;
                default:
                    break;
            }
        }

    }

}

public enum itemtypes
{
    Hp_heal,
    Score_bonus,
    Speed_boost,
    Damage_multiplier,
    Invincibility,
    Cooldown_reduction
}