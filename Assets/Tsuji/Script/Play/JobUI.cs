using UnityEngine;

[CreateAssetMenu(fileName = "JobUI", menuName = "Scriptable Objects/JobUI")]
public class JobUI : ScriptableObject
{


    [NamedArray(new string[] { "attack", "attack2", "skill", "defence","jobIcon" })]
    public  Sprite[] ninja = new Sprite[5];

    [NamedArray(new string[] { "attack", "attack2", "skill", "defence", "jobIcon" })]
    public Sprite[] tonto = new Sprite[5];

    [NamedArray(new string[] { "attack", "attack2", "skill", "defence", "jobIcon" })]
    public Sprite[] marlion = new Sprite[5];

}
