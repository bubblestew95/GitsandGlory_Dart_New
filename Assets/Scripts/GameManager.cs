using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    // 다시 로비로 돌아가는 버튼
    [SerializeField]
    private Button backToLobbyBtn = null;

    // 현재 턴 수
    private int turnCnt = 1;
    // 현재 턴의 플레이어 키값.
    private int curTurnPlayerKeyValue = 1;

    #region CallbackFunctions
    private void Start()
    {
        // Todo : 현재 방의 정보에 따라서 점수판 닉네임 UI를 업데이트한다.
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

        // 턴이 넘어갔으므로, 다음 턴 플레이어를 찾는다.
        do
        {
            ++curTurnPlayerKeyValue;
            // 다음 턴 플레이어 탐색 중에 최대 범위를 벗어나버리면 게임 종료 호출하면서 탈출.
            if (curTurnPlayerKeyValue > PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                EndGame();
                return;
            }

        } while (PhotonNetwork.CurrentRoom.Players.ContainsKey(curTurnPlayerKeyValue));
    }

    // 게임 끝. 게임 기능 비활성화하고 순위를 출력한다.
    private void EndGame()
    {
        // 게임 기능은 비활성화한다.

        // 약간의 시간 뒤에 순위 UI를 출력한다.

    }
}
