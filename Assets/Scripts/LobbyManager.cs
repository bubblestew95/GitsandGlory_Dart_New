using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// 로비와 대기실에서의 서버 기능을 담당하는 매니저 스크립트.
/// </summary>
public class LobbyManager : MonoBehaviourPunCallbacks
{
    // 로비 -> 게임 대기실로 들어가는 버튼
    [SerializeField]
    private Button gameRoomEnterBtn = null;

    // 마스터가 게임 대기실 -> 게임 시작하는 버튼
    [SerializeField]
    private Button gameStartBtn = null;

    // 한 방의 최대 플레이어 수
    private int maxPlayerPerRoom = 4;
    // 게임 버전
    private string gameVersion = "0.0.1";
    // 플레이어의 닉네임
    private string nickName = string.Empty;

    #region CallbackFunctions
    private void Awake()
    {
        // 모든 플레이어는 마스터의 게임 로드 명령에 따라 같은 씬이 로드되야 함.
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnDisconnected(DisconnectCause _cause)
    {
        Debug.LogWarningFormat("Disconnted Cause : {0}", _cause);
    }
    public override void OnConnectedToMaster()
    {
        Debug.LogFormat("Connected To Master : {0}", nickName);

        // Todo : 로비 UI를 끈다.

        // 랜덤한 방으로 들어간다.
        // 만약 들어갈 수 있는 방이 없다면 OnJoinRandomFailed 호출됨.
        PhotonNetwork.JoinRandomRoom();
    }

    // 랜덤 방 입장에 실패했을 때
    public override void OnJoinRandomFailed(short _returnCode, string _message)
    {
        Debug.LogFormat("JoinRandomRoom Failed({0}) : {1}", _returnCode, _message);

        // 새로운 방을 생성한다. 그리고 해당 방의 방장이 된다.
        CreateNewRoom();
    }

    // 방 입장에 성공했을 때
    public override void OnJoinedRoom()
    {
        // Todo List
        // 대기실 UI를 켠다.
        // 대기실의 플레이어 리스트를 갱신한다. (RPC?)

    }
    #endregion

    public void EnterToWaitingRoom()
    {
        // 닉네임을 입력해야 함.
        if (string.IsNullOrEmpty(nickName))
            return;

        // 포톤 네트워크에 접속된 상태 == 어떤 방에서 게임을 끝낸 뒤 다시 로비로 돌아온 상황
        // 새로 접속할 필요는 없으니까 랜덤한 방 하나를 들어간다.
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.LogFormat("Connect : {0}", gameVersion);
            PhotonNetwork.GameVersion = gameVersion;
            
            // 포톤 네트워크로 접속 시도.
            // 접속에 성공하면 OnConnectedToMaster 호출
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 대기실에서 게임을 시작함.
    public void StartGame()
    {
        // 마스터만 게임을 시작할 수 있음.
        if (!PhotonNetwork.IsMasterClient)
            return;

        // 같은 방에 있는 클라이언트들에게 게임 씬을 로드하도록 한다.
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
