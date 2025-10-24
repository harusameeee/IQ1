using UnityEngine;

[CreateAssetMenu(fileName = "Score", menuName = "Scriptable Objects/Score")]
public class Score : ScriptableObject
{
    [SerializeField] int score;
}
