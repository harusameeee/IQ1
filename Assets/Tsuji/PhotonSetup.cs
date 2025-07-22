using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // ★追加
using Photon.Realtime; // ★追加

public class PhotonSetup : MonoBehaviourPunCallbacks
{
    //player
    public GameObject PhotonObject;

    // Start is called before the first frame update
    void Start()
    {
        //サーバーに接続
        PhotonNetwork.ConnectUsingSettings();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //サーバー接続完了
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    //ロビー入室完了
    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    //入室失敗
    // 入室に失敗した場合に呼ばれるコールバック
    // １人目は部屋がないため必ず失敗するので部屋を作成する
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4; // 最大8人まで入室可能
        PhotonNetwork.CreateRoom(null, roomOptions); //第一引数はルーム名
    }


    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(
            PhotonObject.name,
            new Vector3(0f, 1f, 0f),    //ポジション
            Quaternion.identity,    //回転
            0
        );
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        mainCamera.GetComponent<UnityChan.ThirdPersonCamera>().enabled = true;
    }
}
