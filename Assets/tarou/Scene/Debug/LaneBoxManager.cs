using UnityEngine;

public class LaneBoxManager : MonoBehaviour
{
    public Transform[] lanes; // レーンのTransform
    public PlayerLineMove[] players; // プレイヤーPrefabの参照

    // 例えばプレイヤーをレーンに沿って移動
    public void MovePlayerToLane(int playerIndex, int laneIndex)
    {
        if (playerIndex < players.Length && laneIndex < lanes.Length)
        {
            var player = players[playerIndex];
            var lane = lanes[laneIndex];
            Vector3 pos = lane.position; // レーンの位置に移動
            player.transform.position = pos + Vector3.up * 0.5f; // 少し浮かせる
        }
    }
}