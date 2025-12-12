using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "skilldata", menuName = "ScriptableObjects/skilldata", order = 1)]
public class skilldata : ScriptableObject
{
    public string skillname;
    public float cooldown;
    public float currentcooldown;
    public float gcd;
    public int maxstacks=1;
    public int currentstacks;
    public int coincost;
    public bool has_cooldown;
    public int coingain;
    public Sprite icon;
    public List<skilleffect> onHit_effect;
    public List<skilleffect> onUse_effects;
 

}
