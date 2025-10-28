using UnityEngine;

public class playerspawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static playerspawner instance;
    public player_ui[] playeruis = new player_ui[2];

    public Transform[] lanes = new Transform[5];

    void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }
    void spawnchara(class_type ct, int playernum)//note player numn should be 0 or 1
    {
    }
    public enum class_type
    {
        merlion,
        ninja,
        tonto
    }
}
