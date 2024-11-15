using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class GameManager : MonoBehaviourPunCallbacks
{
    private static GameManager instance = null;

    /// <summary>
    /// 조준점 UI 스크립트
    /// </summary>
    [SerializeField]
    private Pointer pointerUI = null;

    private PlayerController[] arr_PlayerCont = null;

    /// <summary>
    /// 이 클라이언트의 플레이어 컨트롤러
    /// </summary>
    private PlayerController playerCont = null;

    /// <summary>
    /// 점수 계산 스크립트
    /// </summary>
    private ScoreEvaluate scoreEvaluate = null;

    /// <summary>
    /// 현재 플레이어들의 각 라운드 당 점수
    /// </summary>
    private int[,] playerScores = null;

    /// <summary>
    /// 각 플레이어의 점수 총합 배열
    /// </summary>
    private int[] scoreTotals = null;

    /// <summary>
    /// 각 플레이어의 인덱스를 등수에 따라 담는 배열
    /// </summary>
    private int[] playerRankIdxs = null;

    /// <summary>
    /// 현재 턴 수
    /// </summary>
    private int turnCnt = 0;
    /// <summary>
    /// 이번 턴에서 던진 다트 수
    /// </summary>
    private int roundCnt = 1;
    /// <summary>
    /// 현재 턴의 플레이어 키값.
    /// </summary>
    private int curTurnPlayerKeyValue = 0;
    /// <summary>
    /// 현재 턴의 플레이어 오브젝트 액터 넘버
    /// </summary>
    private int curTurnPlayerActorNum = 0;

    /// <summary>
    /// 마지막으로 던진 다트의 도착점
    /// </summary>
    private Vector3 lastDartEndPoint = Vector3.zero;

    /// <summary>
    /// 게임 오버 체크
    /// </summary>
    private bool isGameOver = false;
    /// <summary>
    /// 다음 라운드 준비 여부 체크
    /// </summary>
    private bool isNextRoundReady = true;

    private AudioSource audioSrc = null;

    #region Properties
    public static GameManager Instance
    {
        get { return instance; }
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

    public Pointer PointerUI
    {
        get { return pointerUI; }
    }

    public ScoreEvaluate ScoreEvaluate
    {
        get { return scoreEvaluate; }
    }

    public AudioSource AudioSrc
    {
        get { return audioSrc; }
    }
    #endregion

    #region CallbackFunctions
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        arr_PlayerCont = new PlayerController[4];
        scoreEvaluate = new ScoreEvaluate();
        audioSrc = GetComponent<AudioSource>();

        playerScores = new int[4, 3];

        scoreTotals = new int[4];

        playerRankIdxs = new int[4];
    }

    private void Start()
    {
        SpawnPlayer();

        // 게임을 시작하면 턴을 한 번 넘기면서 처음 턴을 시작한다.
        ChangeTurnToNextPlayer();

        // Todo : 현재 방의 정보에 따라서 점수판 닉네임 UI를 업데이트한다.
        for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            UIManager.Instance.UpdatePlayerName(i, PhotonNetwork.CurrentRoom.Players[i + 1].NickName);
        }

        audioSrc.Play();
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

    #region RPC
    // 네트워크의 모든 클라이언트에게 라운드를 넘기라고 지시.
    [PunRPC]
    private void NextRound()
    {
        ++roundCnt;

        Debug.LogFormat("Next Round : {0}", roundCnt <= 3 ? roundCnt : "Round Over");

        StartCoroutine(RoundReadyCoroutine());
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
    public void UpdateLastDartEndPoint(Vector3 _pos)
    {
        lastDartEndPoint = _pos;
    }

    [PunRPC]
    private void UpdateScores(int _score)
    {
        // 스코어 갱신
        playerScores[turnCnt - 1,roundCnt - 1] = _score;

        scoreTotals[turnCnt - 1] += _score;

        // 스코어 UI 업데이트
        UIManager.Instance.UpdatePlayerScore(turnCnt - 1, roundCnt, _score, scoreTotals[turnCnt - 1]);
    }

    #endregion

    // 한 라운드가 끝났음.
    public void RoundComplete(int _score)
    {
        // 모든 클라이언트에게 이번 라운드의 점수를 갱신하라고 한다.
        photonView.RPC("UpdateScores", RpcTarget.All, _score);

        // 모든 클라이언트에게 다음 라운드로 넘어가라고 지시한다.
        photonView.RPC("NextRound", RpcTarget.All);
    }

    // 방을 나가는 함수
    public void LeaveRoom()
    {
        Debug.Log("Leave Room");

        PhotonNetwork.LeaveRoom();
    }

    // 턴을 다음 플레이어에게 넘긴다.
    private void ChangeTurnToNextPlayer()
    {
        // 현재 던져진 다트들을 모두 지운다.
        Dart[] throwedDarts = FindObjectsByType<Dart>(FindObjectsSortMode.None);

        foreach(Dart dart in throwedDarts)
        {
            Destroy(dart.gameObject);
        }

        ++turnCnt;

        // 턴이 넘어갔으므로 라운드를 다시 1로 돌린다.
        roundCnt = 1;

        // 만약 다음 턴이 플레이어의 수보다 더 많다면 플레이어의 수만큼 턴이 돌았다는 뜻
        // 게임이 끝남.
        if (turnCnt > PhotonNetwork.CurrentRoom.PlayerCount)
        {
            EndGame();
            return;
        }

        ++curTurnPlayerKeyValue;

        // 다음 턴 플레이어 컨트롤러 오브젝트의 액터 넘버를 보관한다.
        if (arr_PlayerCont[turnCnt - 1] != null)
        {
            curTurnPlayerActorNum = arr_PlayerCont[turnCnt - 1].photonView.OwnerActorNr;
        }
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

    /// <summary>
    /// 게임 종료 시 UI를 출력해주는 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowGameEndUICoroutine()
    {
        yield return new WaitForSeconds(1.0f);

        // 게임 종료 때 등수를 계산한다.
        for(int i = 0; i < 4; ++i)
        {
            int rankIdx = 0;
            for(int j = 0; j < 4; ++j)
            {
                if (i == j) continue;

                if (scoreTotals[i] < scoreTotals[j])
                    ++rankIdx;
            }

            playerRankIdxs[rankIdx] = i;
        }
        // 계산한 등수를 토대로 게임 오버 UI를 갱신한다.
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            int rankIdx = playerRankIdxs[i];
            UIManager.Instance.SetResultUI(rankIdx, PhotonNetwork.CurrentRoom.Players[rankIdx + 1].NickName, scoreTotals[rankIdx]);
        }

        // 게임 오버 UI 출력
        UIManager.Instance.ShowResultUI();
    }

    private IEnumerator RoundReadyCoroutine()
    {
        isNextRoundReady = false;

        yield return new WaitForSeconds(2.0f);

        isNextRoundReady = true;

        if (roundCnt > 3)
            ChangeTurnToNextPlayer();
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
}
