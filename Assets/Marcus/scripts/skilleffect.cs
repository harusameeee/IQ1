using UnityEngine;
public abstract class skilleffect : ScriptableObject
{
    public abstract void activeeffect(entity user,entity target,skilldata skilldata = null);
}
