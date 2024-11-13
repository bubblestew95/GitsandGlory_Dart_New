using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// �κ�� ���ǿ����� ���� ����� ����ϴ� �Ŵ��� ��ũ��Ʈ.
/// </summary>
public class LobbyManager : MonoBehaviourPunCallbacks
{
    // �κ� -> ���� ���Ƿ� ���� ��ư
    [SerializeField]
    private Button gameRoomEnterBtn = null;

    // �����Ͱ� ���� ���� -> ���� �����ϴ� ��ư
    [SerializeField]
    private Button gameStartBtn = null;

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

        // Todo : �κ� UI�� ����.

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
        // ���� UI�� �Ҵ�.
        // ������ �÷��̾� ����Ʈ�� �����Ѵ�. (RPC?)

    }
    #endregion

    public void EnterToWaitingRoom()
    {
        // �г����� �Է��ؾ� ��.
        if (string.IsNullOrEmpty(nickName))
            return;

        // ���� ��Ʈ��ũ�� ���ӵ� ���� == � �濡�� ������ ���� �� �ٽ� �κ�� ���ƿ� ��Ȳ
        // ���� ������ �ʿ�� �����ϱ� ������ �� �ϳ��� ����.
        if(PhotonNetwork.IsConnected)
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
