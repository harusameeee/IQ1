using UnityEngine;

public interface Damagable
{
    public bool TakeDamage(float damageAmount, bool comboable = true);
    public bool TakeDamage_screenaoe(float damageAmount, hurtbox hb,hurtbox.targetype targettype = hurtbox.targetype.player);

}