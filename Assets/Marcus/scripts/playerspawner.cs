using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class playerspawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static playerspawner instance;
    public hitboxvisualizer hbv;
    public player_ui[] playeruis = new player_ui[2];//ui for player 1 and 2

    public Transform[] lanes = new Transform[5];
    public GameObject[] characters_prefabs = new GameObject[3];
    [HideInInspector] public PlayerLineMove[] players = new PlayerLineMove[2];

    [SerializeField] private SelectedPlayerJob[] playerJob;
    [SerializeField] private GameObject prehub;
    [SerializeField] private Sprite[] playerNumImage=new Sprite[2];


    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        playerspawner.class_type type1 =
        (playerspawner.class_type)System.Enum.Parse(typeof(playerspawner.class_type), playerJob[0].playerJobName, true);
        playerspawner.class_type type2 =
            (playerspawner.class_type)System.Enum.Parse(typeof(playerspawner.class_type), playerJob[1].playerJobName, true);
        spawnchara(type2, 1);
        spawnchara(type1, 0);
        players[0].otherplayer = players[1];
        players[1].otherplayer = players[0];
    }
    void spawnchara(class_type ct, int playernum)// used for spawning characters note player numn should be 0 or 1
    {
        GameObject chara = Instantiate(characters_prefabs[(int)ct], transform);

        GameObject childObject = Instantiate(prehub, chara.transform);
        childObject.transform.localPosition = new Vector3(0, 2, 0);
        childObject.transform.localRotation = Quaternion.identity;
        childObject.transform.localScale = new Vector3(1, 1, 1);

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
        players[playernum] = player;
    }
    public enum class_type
    {
        merlion,
        ninja,
        tonto
    }
}
