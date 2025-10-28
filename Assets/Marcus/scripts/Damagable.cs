using System.Collections.Generic;
using UnityEngine;

public interface Damagable
{
    public bool TakeDamage(float damageAmount, bool comboable = true,List<damagable_type> damagable_types = null);
    public bool TakeDamage_screenaoe(float damageAmount, hurtbox hb, hurtbox.targetype targettype = hurtbox.targetype.player,List<damagable_type> damagable_types = null);

}
public enum damagable_type
{
    poison,
    dmgup,
    weakened,
    poison_deluge,
    blocked
}
