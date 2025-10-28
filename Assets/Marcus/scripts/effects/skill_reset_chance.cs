using UnityEngine;
using Random = System.Random;
[CreateAssetMenu(fileName = "skill_reset_chance", menuName = "ScriptableObjects/Skilleffects/skill_reset_chance", order = 3)]
public class skill_reset_chance : skilleffect
{
    public float reset_chance;
    [Range(0, 3)]
    public int skillindex;

    public override void activeeffect(entity user, entity target, skilldata skilldata = null)
    {

        Random rng = new Random(System.DateTime.Now.Millisecond);
        int rand = rng.Next(0, 100);
        PlayerLineMove pm = user as PlayerLineMove;
        if (rand <= reset_chance && user is PlayerLineMove)
        {
            pm.resetskillcd(skillindex);
            Debug.Log($"P{pm.playerNumber} skill {skillindex} cooldown reset!");
        }
    }
}
