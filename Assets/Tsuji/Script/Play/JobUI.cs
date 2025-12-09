using UnityEngine;

[CreateAssetMenu(fileName = "JobUI", menuName = "Scriptable Objects/JobUI")]
public class JobUI : ScriptableObject
{


    [NamedArray(new string[] { "attack", "attack2", "skill", "defence", "flag", "jobIcon","name"})]
    public  Sprite[] ninja = new Sprite[7];

    [NamedArray(new string[] { "attack", "attack2", "skill", "defence", "flag", "jobIcon","name"})]
    public Sprite[] tonto = new Sprite[7];

    [NamedArray(new string[] { "attack", "attack2", "skill", "defence", "flag", "jobIcon" , "name"})]
    public Sprite[] marlion = new Sprite[7];

}
