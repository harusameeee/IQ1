// 例：スコアを加算するスクリプト
using UnityEngine;

public class AddScore : MonoBehaviour
{
    [SerializeField] private ScoreData scoreData;

    public void Add(int amount)
    {
        scoreData.score += amount; // 自動でOnScoreChangedが呼ばれる
    }
}
