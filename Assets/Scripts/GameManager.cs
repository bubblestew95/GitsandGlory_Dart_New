using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class GameManager : MonoBehaviourPunCallbacks
{
    private static GameManager instance = null;

    // 다시 로비로 돌아가는 버튼
    [SerializeField]
    private Button backToLobbyBtn = null;

    // 플레이어 컨트롤러 오브젝트 배열
    [SerializeField]
    private PlayerController[] arr_PlayerCont = null;

    // 이 클라이언트의 플레이어 컨트롤러
    private PlayerController playerCont = null;

    // 현재 턴 수
    private int turnCnt = 0;
    // 이번 턴에서 던진 다트 수
    private int roundCnt = 1;
    // 현재 턴의 플레이어 키값.
    private int curTurnPlayerKeyValue = 1;
    // 현재 턴의 플레이어 오브젝트 액터 넘버
    private int curTurnPlayerActorNum = 0;

    // 마지막으로 던진 다트
    private Dart lastThrowedDart = null;

    // 게임 오버 체크
    private bool isGameOver = false;
    // 다음 라운드 준비 여부 체크
    private bool isNextRoundReady = true;

    public static GameManager Instance
    {
        get { return instance; }
    }

    public Dart LastThrowedDart
    {
        get { return lastThrowedDart; }
    }

    public bool IsGameOver
    {
        get { return isGameOver; }
    }

    public bool IsNextRoundReady
    {
        get { return isNextRoundReady; }
    }

    public int CurrentTurnPlayerActorNum
    {
        get{ return curTurnPlayerActorNum; }
    }

    #region CallbackFunctions
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        arr_PlayerCont = new PlayerController[4];
    }

    private void Start()
    {
        SpawnPlayer();

        // 게임을 시작하면 턴을 한 번 넘기면서 처음 턴을 시작한다.
        ChangeTurnToNextPlayer();

        // Todo : 현재 방의 정보에 따라서 점수판 닉네임 UI를 업데이트한다.

    }

    private void Update()
    {
        // 게임이 끝난 상태면 리턴
        if (isGameOver) return;

        // 턴을 잡은 캐릭터가 도중에 나갈 경우 처리해야 함.
        if (!PhotonNetwork.CurrentRoom.Players.ContainsKey(curTurnPlayerKeyValue))
            return;
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

    public void RoundComplete()
    {
        photonView.RPC("NextRound", RpcTarget.All);
    }

    // 방을 나가는 함수
    public void LeaveRoom()
    {
        Debug.Log("Leave Room");

        PhotonNetwork.LeaveRoom();
    }

    // 턴을 다음 플레이어에게 넘긴다.
    public void ChangeTurnToNextPlayer()
    {
        ++turnCnt;
        roundCnt = 1;

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

        // 다음 턴 플레이어 컨트롤러 오브젝트의 액터 넘버를 보관한다.
        curTurnPlayerActorNum = arr_PlayerCont[turnCnt - 1].photonView.OwnerActorNr;
    }

    // 네트워크의 모든 클라이언트에게 라운드를 넘기라고 지시.
    [PunRPC]
    private void NextRound()
    {
        ++roundCnt;

        Debug.LogFormat("Next Round : {0}", roundCnt);

        StartCoroutine(RoundReadyCoroutine());

        if (roundCnt > 3)
            ChangeTurnToNextPlayer();
    }

    // 게임 끝. 게임 기능 비활성화하고 순위를 출력한다.
    private void EndGame()
    {
        Debug.Log("Game End!");

        // 게임 기능은 비활성화한다.
        isGameOver = true;

        // 약간의 시간 뒤에 순위 UI를 출력한다.
        StartCoroutine(ShowGameEndUICoroutine());
    }

    // 게임 종료 시 UI를 출력해주는 코루틴
    private IEnumerator ShowGameEndUICoroutine()
    {
        yield return new WaitForSeconds(2.0f);

        // 게임 오버 UI 출력
    }

    private IEnumerator RoundReadyCoroutine()
    {
        isNextRoundReady = false;

        yield return new WaitForSeconds(3.0f);

        isNextRoundReady = true;
    }

    private void SpawnPlayer()
    {
        GameObject go = PhotonNetwork.Instantiate(
                "P_PlayerController",
                Vector3.zero,
                Quaternion.identity,
                0);

        playerCont = go.GetComponent<PlayerController>();

        photonView.RPC("ApplyPlayerList", RpcTarget.All);
    }

    [PunRPC]
    public void ApplyPlayerList()
    {
        // 현재 방에 접속해 있는 플레이어의 수
        Debug.Log("CurrentRoom PlayerCount : " + PhotonNetwork.CurrentRoom.PlayerCount);

        // 현재 생성된 모든 포톤 뷰 가져오기
        PhotonView[] photonViews = FindObjectsByType<PhotonView>(FindObjectsSortMode.None);

        System.Array.Clear(arr_PlayerCont, 0, arr_PlayerCont.Length);

        // 현재 생성된 포톤 뷰 전체와 접속중인 플레이어들의 액터 넘버를 비교해서,
        // 액터 넘버를 기준으로 플레이어 게임 오브젝트 배열을 채움.
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            // 키는 0이 아닌 1부터 시작
            int key = i + 1;

            for (int j = 0; j < photonViews.Length; ++j)
            {
                if (photonViews[j].isRuntimeInstantiated == false) continue;
                if (PhotonNetwork.CurrentRoom.Players.ContainsKey(key) == false) continue;

                int viewNum = photonViews[j].Owner.ActorNumber;
                int playerNum = PhotonNetwork.CurrentRoom.Players[key].ActorNumber;

                // 액터 넘버가 같은 오브젝트가 있다면 플레이어 오브젝트라는 게 판정된다.
                if (viewNum == playerNum)
                {
                    // 실제 게임 오브젝트를 배열에 추가
                    arr_PlayerCont[playerNum - 1] = photonViews[j].gameObject.GetComponent<PlayerController>();
                    // 게임 오브젝트 이름도 알아보기 쉽게 변경
                    arr_PlayerCont[playerNum - 1].gameObject.name = "Player_" + photonViews[j].Owner.NickName;
                }
            }
        }
    }

    [PunRPC]
    public void UpdateLastDart()
    {

    }
}
