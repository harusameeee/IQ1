using UnityEngine;

[CreateAssetMenu(fileName = "SelectedStage", menuName = "Scriptable Objects/SelectedStage")]
public class SelectedStage : ScriptableObject
{
    [SerializeField] private string stageName;

    public string StageName => stageName;

    public void SetStageName(string newName)
    {
        stageName = newName;
        Debug.Log($"ステージデータ更新: {stageName}");
    }
}
