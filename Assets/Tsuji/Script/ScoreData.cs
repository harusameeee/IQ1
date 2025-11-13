using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ScoreData", menuName = "Game/ScoreData")]
public class ScoreData : ScriptableObject
{
    [SerializeField] private int _score;

    // スコア変更イベント
    public event Action<int> OnScoreChanged;

    public int score
    {
        get => _score;
        set
        {
            if (_score != value)
            {
                _score = value;
                OnScoreChanged?.Invoke(_score); // イベント発火
            }
        }
    }

    // デバッグ用（リセットなど）
    public void ResetScore()
    {
        score = 0;
    }
}
