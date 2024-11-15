using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// �κ�� ���ǿ����� ���� ����� ����ϴ� �Ŵ��� ��ũ��Ʈ.
/// </summary>
public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button titleToRoomBtn = null;

    [SerializeField]
    private GameObject titleGo = null;

    [SerializeField]
    private GameObject lobbyGo = null;

    [SerializeField]
    private LobbyUIManager lobbyUIManager = null;

    // �� ���� �ִ� �÷��̾� ��
    private int maxPlayerPerRoom = 4;
    // ���� ����
    private string gameVersion = "0.0.1";
    // �÷��̾��� �г���
    private string nickName = string.Empty;

    #region CallbackFunctions
    private void Awake()
    {
        // ��� �÷��̾�� �������� ���� �ε� ��ɿ� ���� ���� ���� �ε�Ǿ� ��.
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnDisconnected(DisconnectCause _cause)
    {
        Debug.LogWarningFormat("Disconnted Cause : {0}", _cause);
    }
    public override void OnConnectedToMaster()
    {
        Debug.LogFormat("Connected To Master : {0}", nickName);

        // ������ ������ ����.
        // ���� �� �� �ִ� ���� ���ٸ� OnJoinRandomFailed ȣ���.
        PhotonNetwork.JoinRandomRoom();
    }

    // ���� �� ���忡 �������� ��
    public override void OnJoinRandomFailed(short _returnCode, string _message)
    {
        Debug.LogFormat("JoinRandomRoom Failed({0}) : {1}", _returnCode, _message);

        // ���ο� ���� �����Ѵ�. �׸��� �ش� ���� ������ �ȴ�.
        CreateNewRoom();
    }

    // �� ���忡 �������� ��
    public override void OnJoinedRoom()
    {
        // Todo List
        // Ÿ��Ʋ UI�� ����.
        titleGo?.SetActive(false);
        // �κ� UI�� �Ҵ�.
        lobbyGo?.SetActive(true);
        // �κ��� �÷��̾� ����Ʈ�� �����Ѵ�.
        photonView.RPC("UpdateNicknameUIs", RpcTarget.All);
    }

    public override void OnLeftRoom()
    {
        photonView.RPC("UpdateNicknameUIs", RpcTarget.Others);
    }
    #endregion

    // ���Ƿ� ����
    public void EnterToWaitingRoom()
    {
        nickName = lobbyUIManager.CurNickName;

        // �г����� �Է��ؾ� ��.
        if (string.IsNullOrEmpty(nickName))
            return;

        PhotonNetwork.NickName = nickName;
        titleToRoomBtn.interactable = false;

        // ���� ��Ʈ��ũ�� ���ӵ� ���� == � �濡�� ������ ���� �� �ٽ� �κ�� ���ƿ� ��Ȳ
        // ���� ������ �ʿ�� �����ϱ� ������ �� �ϳ��� ����.
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.LogFormat("Connect : {0}", gameVersion);
            PhotonNetwork.GameVersion = gameVersion;

            // ���� ��Ʈ��ũ�� ���� �õ�.
            // ���ӿ� �����ϸ� OnConnectedToMaster ȣ��
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // ���ǿ��� ������ ������.
    public void StartGame()
    {
        // �����͸� ������ ������ �� ����.
        if (!PhotonNetwork.IsMasterClient)
            return;

        // ���� �濡 �ִ� Ŭ���̾�Ʈ�鿡�� ���� ���� �ε��ϵ��� �Ѵ�.
        PhotonNetwork.LoadLevel("Game");
    }


    [PunRPC]
    public void UpdateNicknameUIs()
    {
        string[] curRoomNickNames = new string[maxPlayerPerRoom];
        int myIdx = 0;

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            curRoomNickNames[i] = PhotonNetwork.PlayerList[i].NickName;
            if (photonView.IsMine) myIdx = i;
            Debug.Log(curRoomNickNames[i]);
        }

        lobbyUIManager.UpdateNicknameUIs(curRoomNickNames, myIdx);
    }

    private void CreateNewRoom()
    {
        Debug.Log("Create Room");
        PhotonNetwork.CreateRoom(null,
            new RoomOptions
            {
                MaxPlayers = maxPlayerPerRoom
            });
    }
}
