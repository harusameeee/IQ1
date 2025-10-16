using UnityEngine;
[System.Serializable]
public class skilldata
{
    public string skillname;
    public float cooldown;
    public float currentcooldown;
    public float gcd;
    public int maxstacks;
    public int currentstacks;
    public int coincost;
    public bool has_cooldown;
    public Sprite icon;
}
