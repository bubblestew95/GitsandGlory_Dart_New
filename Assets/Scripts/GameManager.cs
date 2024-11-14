using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button gameExitBtn = null;

    // 현재 턴
    private int turnCnt = 1;
    // 현재 턴의 플레이어 키값.
    private int curTurnPlayerNum = 1;

    #region CallbackFunctions
    private void Start()
    {
        // Todo : 현재 플레이어의 정보에 따라서 점수판 UI를 업데이트한다. (닉네임...)
    }

    public override void OnPlayerLeftRoom(Player _otherPlayer)
    {
        Debug.LogFormat("Player Left Room : {0}", _otherPlayer.NickName);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");

        PhotonNetwork.Disconnect();

        SceneManager.LoadScene("Title_Lobby");
    }
    #endregion

    // 방을 나가는 함수
    public void LeaveRoom()
    {
        Debug.Log("Leave Room");

        PhotonNetwork.LeaveRoom();
    }

    // 턴을 다음 플레이어에게 넘긴다.
    [PunRPC]
    public void ChangeTurnToNextPlayer()
    {
        ++turnCnt;

        // 만약 다음 턴이 플레이어의 수보다 더 많다면 플레이어의 수만큼 턴이 돌았다는 뜻
        if (turnCnt > PhotonNetwork.CurrentRoom.PlayerCount)
        {
            EndGame();
            return;
        }

        do
        {
            ++curTurnPlayerNum;
            if (curTurnPlayerNum > PhotonNetwork.CurrentRoom.MaxPlayers) return;

        } while (PhotonNetwork.CurrentRoom.Players.ContainsKey(curTurnPlayerNum));
    }

    private void EndGame()
    {
        // 게임 기능은 비활성화한다.

        // 약간의 시간 뒤에 순위 UI를 출력한다.

    }
}
