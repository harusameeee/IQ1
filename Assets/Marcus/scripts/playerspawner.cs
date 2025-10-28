using Unity.Mathematics;
using UnityEngine;

public class playerspawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static playerspawner instance;
    public hitboxvisualizer hbv;
    public player_ui[] playeruis = new player_ui[2];

    public Transform[] lanes = new Transform[5];
    public GameObject[] characters_prefabs = new GameObject[3];

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        spawnchara(class_type.merlion, 0);
        spawnchara(class_type.ninja, 1);
    }
    void spawnchara(class_type ct, int playernum)//note player numn should be 0 or 1
    {
        GameObject chara = Instantiate(characters_prefabs[(int)ct], transform);
        if (playernum == 1)
        {

            chara.transform.localPosition = new float3(lanes[2].localPosition.x, 0, 0);
        }
        else
        {
            chara.transform.localPosition = new float3(lanes[4].localPosition.x, 0, 0);
        }
        chara.transform.localRotation = quaternion.Euler(0, math.PI, 0);
        PlayerLineMove player = chara.GetComponent<PlayerLineMove>();
        hbv.additionalhitboxes.Add(new hitboxvisualizer.hitboxpair { todraw = player, hbcolor = Color.green });
        hbv.additionalhitboxes.Add(new hitboxvisualizer.hitboxpair { todraw = player.hb, hbcolor = Color.red });
        player.playerNumber = playernum + 1;
        player.ui = playeruis[playernum];
        player.lanes = lanes;
    }
    public enum class_type
    {
        merlion,
        ninja,
        tonto
    }
}
