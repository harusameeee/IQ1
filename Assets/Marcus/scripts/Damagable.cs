using UnityEngine;

public interface Damagable
{
    public bool TakeDamage(int damageAmount);
    public bool TakeDamage_screenaoe(int damageAmount, hurtbox hb,hurtbox.targetype targettype = hurtbox.targetype.player);

}