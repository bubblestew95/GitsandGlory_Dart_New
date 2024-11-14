using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UnityEngine.InputSystem;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks
{
    // 다시 로비로 돌아가는 버튼
    [SerializeField]
    private Button backToLobbyBtn = null;

    // 다트 프리팹. 싱글에서의 테스트가 끝나면 필요 없음.
    [SerializeField]
    private GameObject dartPrefab = null;

    // 현재 턴 수
    private int turnCnt = 1;
    // 이번 턴에서 던진 다트 수
    private int roundCnt = 1;
    // 현재 턴의 플레이어 키값.
    private int curTurnPlayerKeyValue = 1;

    private bool isGameOver = false;

    #region CallbackFunctions
    private void Start()
    {
        // Todo : 현재 방의 정보에 따라서 점수판 닉네임 UI를 업데이트한다.
    }

    private void Update()
    {
        // 게임이 끝난 상태면 리턴
        if (isGameOver) return;

        // 원격 클라이언트의 게임 매니저라면 리턴
        if (!photonView.IsMine)
            return;

        // 이 클라이언트의 플레이어의 턴이 아니라면 리턴
        if (photonView.Owner.ActorNumber != 
            PhotonNetwork.CurrentRoom.Players[curTurnPlayerKeyValue].ActorNumber)
        {
            return;
        }

        // 마우스 왼 쪽 버튼 클릭 시 다트를 던진다.
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 dartThrowPos = Vector3.zero;

            {
                dartThrowPos = Camera.main.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y,
                    -Camera.main.transform.position.z)
                    );
                dartThrowPos.z = -20f;
            }// 스크린 상 마우스 포지션을 다트 투척 시작점으로 설정. 추후 조준점 UI 생길 시 해당 위치로 업데이트 예정.

            ThrowDart(dartThrowPos);

            // 다트를 던지면 라운드를 1 올린다.
            ++roundCnt;

            // 3라운드가 지난다면 턴이 넘어간다.
            if(roundCnt > 3)
            {
                ChangeTurnToNextPlayer();
            }
        }
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

    /// <summary>
    /// 다트 오브젝트를 생성 후 Dart 스크립트의 ThrowDart 메소드를 호출한다.
    /// </summary>
    /// <param name="_spawnPos">다트의 시작 지점.</param>
    public void ThrowDart(Vector3 _spawnPos)
    {
        // GameObject dartGo = PhotonNetwork.Instantiate("P_Dart", _spawnPos, Quaternion.identity);
        GameObject dartGo = Instantiate<GameObject>(dartPrefab, _spawnPos, Quaternion.identity);
        Dart dart = dartGo.GetComponent<Dart>();

        dart.ThrowDart(_spawnPos, new Vector3(_spawnPos.x, _spawnPos.y, 0f));
    }

    // 턴을 다음 플레이어에게 넘긴다.
    [PunRPC]
    public void ChangeTurnToNextPlayer()
    {
        ++turnCnt;

        // 만약 다음 턴이 플레이어의 수보다 더 많다면 플레이어의 수만큼 턴이 돌았다는 뜻
        // 게임이 끝남.
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
        Debug.Log("Game End!");

        // 게임 기능은 비활성화한다.
        isGameOver = true;

        // 약간의 시간 뒤에 순위 UI를 출력한다.

    }
}
