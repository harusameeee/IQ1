using UnityEngine;
[CreateAssetMenu(fileName = "buffdata", menuName = "ScriptableObjects/buffdata", order = 1)]
public class buffdata : ScriptableObject
{
    public bool stackable;
    public Sprite icon;
    public string buffname;
    public bufftypes type;
    public float pow;
    public float duration;
    public buffdata copy()
    {
        buffdata newbuff = CreateInstance<buffdata>();
        newbuff.stackable = stackable;
        newbuff.icon = icon;
        newbuff.buffname = buffname;
        newbuff.type = type;
        newbuff.pow = pow;
        newbuff.duration = duration;
        return newbuff;
    }
}
public enum bufftypes
{
    gcd_reduction,
    cooldown_reduction,
    speed_increase,//speed increase buff
    attack,//attack buff
    vulnerability,//takes more damage
    stealth,//invuln+ damage boost when hitting from stealth
    invuln///completely invulnerable
    , poison,
    sticktogether,
    stayaway,
    nojump,
    keep_moving,
    Stop_moving


}