using UnityEngine;

[CreateAssetMenu(fileName = "JobUI", menuName = "Scriptable Objects/JobUI")]
public class JobUI : ScriptableObject
{


    [NamedArray(new string[] { "attack", "attack2", "skill", "defence", "flag", "jobIcon","name",
        "setumei_attack","setumei_attack2","setumei_skill","setumei_defence"})]
    public  Sprite[] jobUI= new Sprite[11];


}
