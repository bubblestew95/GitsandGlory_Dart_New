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
    /// ������ UI ��ũ��Ʈ
    /// </summary>
    [SerializeField]
    private Pointer pointerUI = null;

    private PlayerController[] arr_PlayerCont = null;

    /// <summary>
    /// �� Ŭ���̾�Ʈ�� �÷��̾� ��Ʈ�ѷ�
    /// </summary>
    private PlayerController playerCont = null;

    /// <summary>
    /// ���� ��� ��ũ��Ʈ
    /// </summary>
    private ScoreEvaluate scoreEvaluate = null;

    /// <summary>
    /// ���� �÷��̾���� �� ���� �� ����
    /// </summary>
    private int[,] playerScores = null;

    /// <summary>
    /// �� �÷��̾��� ���� ���� �迭
    /// </summary>
    private int[] scoreTotals = null;

    /// <summary>
    /// �� �÷��̾��� �ε����� ����� ���� ��� �迭
    /// </summary>
    private int[] playerRankIdxs = null;

    /// <summary>
    /// ���� �� ��
    /// </summary>
    private int turnCnt = 0;
    /// <summary>
    /// �̹� �Ͽ��� ���� ��Ʈ ��
    /// </summary>
    private int roundCnt = 1;
    /// <summary>
    /// ���� ���� �÷��̾� Ű��.
    /// </summary>
    private int curTurnPlayerKeyValue = 0;
    /// <summary>
    /// ���� ���� �÷��̾� ������Ʈ ���� �ѹ�
    /// </summary>
    private int curTurnPlayerActorNum = 0;

    /// <summary>
    /// ���������� ���� ��Ʈ�� ������
    /// </summary>
    private Vector3 lastDartEndPoint = Vector3.zero;

    /// <summary>
    /// ���� ���� üũ
    /// </summary>
    private bool isGameOver = false;
    /// <summary>
    /// ���� ���� �غ� ���� üũ
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

        // ������ �����ϸ� ���� �� �� �ѱ�鼭 ó�� ���� �����Ѵ�.
        ChangeTurnToNextPlayer();

        // Todo : ���� ���� ������ ���� ������ �г��� UI�� ������Ʈ�Ѵ�.
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
    // ��Ʈ��ũ�� ��� Ŭ���̾�Ʈ���� ���带 �ѱ��� ����.
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
        // ���� �濡 ������ �ִ� �÷��̾��� ��
        Debug.Log("CurrentRoom PlayerCount : " + PhotonNetwork.CurrentRoom.PlayerCount);

        // ���� ������ ��� ���� �� ��������
        PhotonView[] photonViews = FindObjectsByType<PhotonView>(FindObjectsSortMode.None);

        System.Array.Clear(arr_PlayerCont, 0, arr_PlayerCont.Length);

        // ���� ������ ���� �� ��ü�� �������� �÷��̾���� ���� �ѹ��� ���ؼ�,
        // ���� �ѹ��� �������� �÷��̾� ���� ������Ʈ �迭�� ä��.
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            // Ű�� 0�� �ƴ� 1���� ����
            int key = i + 1;

            for (int j = 0; j < photonViews.Length; ++j)
            {
                if (photonViews[j].isRuntimeInstantiated == false) continue;
                if (PhotonNetwork.CurrentRoom.Players.ContainsKey(key) == false) continue;

                int viewNum = photonViews[j].Owner.ActorNumber;
                int playerNum = PhotonNetwork.CurrentRoom.Players[key].ActorNumber;

                // ���� �ѹ��� ���� ������Ʈ�� �ִٸ� �÷��̾� ������Ʈ��� �� �����ȴ�.
                if (viewNum == playerNum)
                {
                    // ���� ���� ������Ʈ�� �迭�� �߰�
                    arr_PlayerCont[playerNum - 1] = photonViews[j].gameObject.GetComponent<PlayerController>();
                    // ���� ������Ʈ �̸��� �˾ƺ��� ���� ����
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
        // ���ھ� ����
        playerScores[turnCnt - 1,roundCnt - 1] = _score;

        scoreTotals[turnCnt - 1] += _score;

        // ���ھ� UI ������Ʈ
        UIManager.Instance.UpdatePlayerScore(turnCnt - 1, roundCnt, _score, scoreTotals[turnCnt - 1]);
    }

    #endregion

    // �� ���尡 ������.
    public void RoundComplete(int _score)
    {
        // ��� Ŭ���̾�Ʈ���� �̹� ������ ������ �����϶�� �Ѵ�.
        photonView.RPC("UpdateScores", RpcTarget.All, _score);

        // ��� Ŭ���̾�Ʈ���� ���� ����� �Ѿ��� �����Ѵ�.
        photonView.RPC("NextRound", RpcTarget.All);
    }

    // ���� ������ �Լ�
    public void LeaveRoom()
    {
        Debug.Log("Leave Room");

        PhotonNetwork.LeaveRoom();
    }

    // ���� ���� �÷��̾�� �ѱ��.
    private void ChangeTurnToNextPlayer()
    {
        // ���� ������ ��Ʈ���� ��� �����.
        Dart[] throwedDarts = FindObjectsByType<Dart>(FindObjectsSortMode.None);

        foreach(Dart dart in throwedDarts)
        {
            Destroy(dart.gameObject);
        }

        ++turnCnt;

        // ���� �Ѿ���Ƿ� ���带 �ٽ� 1�� ������.
        roundCnt = 1;

        // ���� ���� ���� �÷��̾��� ������ �� ���ٸ� �÷��̾��� ����ŭ ���� ���Ҵٴ� ��
        // ������ ����.
        if (turnCnt > PhotonNetwork.CurrentRoom.PlayerCount)
        {
            EndGame();
            return;
        }

        ++curTurnPlayerKeyValue;

        // ���� �� �÷��̾� ��Ʈ�ѷ� ������Ʈ�� ���� �ѹ��� �����Ѵ�.
        if (arr_PlayerCont[turnCnt - 1] != null)
        {
            curTurnPlayerActorNum = arr_PlayerCont[turnCnt - 1].photonView.OwnerActorNr;
        }
    }

    

    // ���� ��. ���� ��� ��Ȱ��ȭ�ϰ� ������ ����Ѵ�.
    private void EndGame()
    {
        Debug.Log("Game End!");

        // ���� ����� ��Ȱ��ȭ�Ѵ�.
        isGameOver = true;

        // �ణ�� �ð� �ڿ� ���� UI�� ����Ѵ�.
        StartCoroutine(ShowGameEndUICoroutine());
    }

    /// <summary>
    /// ���� ���� �� UI�� ������ִ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowGameEndUICoroutine()
    {
        yield return new WaitForSeconds(1.0f);

        // ���� ���� �� ����� ����Ѵ�.
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
        // ����� ����� ���� ���� ���� UI�� �����Ѵ�.
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            int rankIdx = playerRankIdxs[i];
            UIManager.Instance.SetResultUI(rankIdx, PhotonNetwork.CurrentRoom.Players[rankIdx + 1].NickName, scoreTotals[rankIdx]);
        }

        // ���� ���� UI ���
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
