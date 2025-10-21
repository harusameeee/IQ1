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
    public Sprite icon;
    public virtual void skilleffect(PlayerLineMove user)
    {
        
    }
}
