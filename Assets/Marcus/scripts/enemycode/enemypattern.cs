using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "enemypattern", menuName = "ScriptableObjects/enemypattern", order = 1)]
public class enemypattern : ScriptableObject
{
    public List<enemypatterndata> patterndata = new List<enemypatterndata>();
}
[System.Serializable]
public class enemypatterndata
{
    public GameObject attackhbobj;
    public Vector2 offset = Vector2.zero;
}
