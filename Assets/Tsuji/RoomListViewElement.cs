using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListViewElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameLabel = default;
    [SerializeField]
    private TextMeshProUGUI playerCounter = default;

    private Matchmaking matchmakingView;
    private Button button;

    public void Init(Matchmaking parentView)
    {
        matchmakingView = parentView;

        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // ルーム参加処理中は、入力できないようにする
        matchmakingView.OnJoinedRoom();

        // ボタンに対応したルーム名のルームに参加する
        PhotonNetwork.JoinRoom(nameLabel.text);
    }

    public void Show(RoomInfo roomInfo)
    {
        nameLabel.text = roomInfo.Name;
        playerCounter.SetText("{0} / {1}", roomInfo.PlayerCount, roomInfo.MaxPlayers);

        // ルームが満員でない時のみ、参加ボタンを押せるようにする
        button.interactable = (roomInfo.PlayerCount < roomInfo.MaxPlayers);

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}