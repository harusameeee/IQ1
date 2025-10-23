using UnityEngine;
[CreateAssetMenu(fileName = "inflictbuff", menuName = "ScriptableObjects/Skilleffects/inflictbuff", order = 2)]
public class inflictbuff : skilleffect
{
    public entity.buffdata buff;
    public bool selftarget;

    public override void activeeffect(entity user, entity target)
    {
        entity actualTarget = selftarget ? user : target;
        var existingBuff = actualTarget.buffs.Find(x => x.buffname == buff.buffname);
        if (existingBuff != null )
        {
            if (!buff.stackable) return;
            existingBuff.pow += buff.pow;
            existingBuff.duration = Mathf.Max(existingBuff.duration, buff.duration);
        }
        else
        {
            actualTarget.buffs.Add(buff.copy());
        }
    }
}
