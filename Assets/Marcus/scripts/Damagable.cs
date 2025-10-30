using System.Collections.Generic;
using UnityEngine;

public interface Damagable
{
    public bool TakeDamage(float damageAmount, bool comboable = true, List<damagable_type> damagable_Types = null, Vector2 hitpoint = new Vector2());
    public bool  TakeDamage_screenaoe(float damageAmount, hurtbox hb, hurtbox.targetype targettype,out Rect overlap,List<damagable_type> damagable_Types = null);

}
public enum damagable_type
{
    poison,
    dmgup,
    weakened,
    poison_deluge,
    blocked
}
