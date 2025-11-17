using UnityEngine;
[CreateAssetMenu(fileName = "inflictbuff", menuName = "ScriptableObjects/Skilleffects/inflictbuff", order = 2)]
public class inflictbuff : skilleffect
{
    
    public buffdata buff;
    public bool selftarget;

    public override void activeeffect(entity user, entity target,skilldata skilldata = null)
    {
        Debug.Log("inflict buff");
        entity actualTarget = selftarget ? user : target;
        actualTarget.addbuff(buff);
    }
}
