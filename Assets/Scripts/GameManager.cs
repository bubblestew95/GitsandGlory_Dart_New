using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class GameManager : MonoBehaviourPunCallbacks
{
    private static GameManager instance = null;

    // �ٽ� �κ�� ���ư��� ��ư
    [SerializeField]
    private Button backToLobbyBtn = null;

    // �÷��̾� ��Ʈ�ѷ� ������Ʈ �迭
    [SerializeField]
    private PlayerController[] arr_PlayerCont = null;

    // �� Ŭ���̾�Ʈ�� �÷��̾� ��Ʈ�ѷ�
    private PlayerController playerCont = null;

    // ���� �� ��
    private int turnCnt = 0;
    // �̹� �Ͽ��� ���� ��Ʈ ��
    private int roundCnt = 1;
    // ���� ���� �÷��̾� Ű��.
    private int curTurnPlayerKeyValue = 1;
    // ���� ���� �÷��̾� ������Ʈ ���� �ѹ�
    private int curTurnPlayerActorNum = 0;

    // ���������� ���� ��Ʈ
    private Dart lastThrowedDart = null;

    // ���� ���� üũ
    private bool isGameOver = false;
    // ���� ���� �غ� ���� üũ
    private bool isNextRoundReady = true;

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

        // ������ �����ϸ� ���� �� �� �ѱ�鼭 ó�� ���� �����Ѵ�.
        ChangeTurnToNextPlayer();

        // Todo : ���� ���� ������ ���� ������ �г��� UI�� ������Ʈ�Ѵ�.

    }

    private void Update()
    {
        // ������ ���� ���¸� ����
        if (isGameOver) return;

        // ���� ���� ĳ���Ͱ� ���߿� ���� ��� ó���ؾ� ��.
        if (!PhotonNetwork.CurrentRoom.Players.ContainsKey(curTurnPlayerKeyValue))
            return;


        //// ���콺 �� �� ��ư Ŭ�� �� ��Ʈ�� ������.
        //if (Input.GetMouseButtonDown(0))
        //{
        //    // ���� ���尡 �غ���� �ʾҴٸ� ����
        //    if (!isNextRoundReady)
        //        return;

        //    // ���� �����. ���� ����� ���� �غ���� ����.
        //    isNextRoundReady = false;

        //    Vector3 dartThrowPos = Vector3.zero;

        //    {
        //        dartThrowPos = Camera.main.ScreenToWorldPoint(
        //            new Vector3(Input.mousePosition.x,
        //            Input.mousePosition.y,
        //            -Camera.main.transform.position.z)
        //            );
        //        dartThrowPos.z = -20f;
        //    }// ��ũ�� �� ���콺 �������� ��Ʈ ��ô ���������� ����. ���� ������ UI ���� �� �ش� ��ġ�� ������Ʈ ����.
            
        //    // ��Ʈ��ũ�� ���ؼ� ��� ������Ʈ���� ��Ʈ �����ϵ��� ���.
        //    GameObject dartGo = PhotonNetwork.Instantiate("P_Dart", Vector3.zero, Quaternion.identity);

        //    photonView.RPC("ThrowDart", RpcTarget.All, dartGo.GetComponent<Dart>(), dartThrowPos);

        //    // ��Ʈ�� ������ ���带 1 �ø���.
        //    ++roundCnt;

        //    // 3���尡 �����ٸ� ���� �Ѿ��.
        //    if(roundCnt > 3)
        //    {
        //        photonView.RPC("ChangeTurnToNextPlayer", RpcTarget.All);
        //    }

        //    StartCoroutine(RoundReadyCoroutine());
        //}
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

    // ���� ������ �Լ�
    public void LeaveRoom()
    {
        Debug.Log("Leave Room");

        PhotonNetwork.LeaveRoom();
    }

    ///// <summary>
    ///// ��Ʈ ������Ʈ�� ���� �� Dart ��ũ��Ʈ�� ThrowDart �޼ҵ带 ȣ���Ѵ�.
    ///// </summary>
    ///// <param name="_spawnPos">��Ʈ�� ���� ����.</param>
    //[PunRPC]
    //public void ThrowDart(Dart _dart, Vector3 _spawnPos)
    //{
    //    lastThrowedDart = _dart;

    //    lastThrowedDart.ThrowDart(_spawnPos, new Vector3(_spawnPos.x, _spawnPos.y, 0f));
    //}

    //// �������� ���� ��Ʈ�� ������ ��ǥ
    //public Vector2 GetLastDartEndPos()
    //{
    //    return lastThrowedDart.EndPosition;
    //}

    // ���� ���� �÷��̾�� �ѱ��.
    public void ChangeTurnToNextPlayer()
    {
        ++turnCnt;
        roundCnt = 1;

        // ���� ���� ���� �÷��̾��� ������ �� ���ٸ� �÷��̾��� ����ŭ ���� ���Ҵٴ� ��
        // ������ ����.
        if (turnCnt > PhotonNetwork.CurrentRoom.PlayerCount)
        {
            EndGame();
            return;
        }

        // ���� �Ѿ���Ƿ�, ���� �� �÷��̾ ã�´�.
        do
        {
            ++curTurnPlayerKeyValue;
            // ���� �� �÷��̾� Ž�� �߿� �ִ� ������ ��������� ���� ���� ȣ���ϸ鼭 Ż��.
            if (curTurnPlayerKeyValue > PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                EndGame();
                return;
            }

        } while (PhotonNetwork.CurrentRoom.Players.ContainsKey(curTurnPlayerKeyValue));

        // ���� �� �÷��̾� ��Ʈ�ѷ� ������Ʈ�� ���� �ѹ��� �����Ѵ�.
        curTurnPlayerActorNum = arr_PlayerCont[turnCnt - 1].photonView.OwnerActorNr;
    }

    // ��Ʈ��ũ�� ��� Ŭ���̾�Ʈ���� ���带 �ѱ��� ����.
    [PunRPC]
    private void NextRound()
    {
        ++roundCnt;

        StartCoroutine(RoundReadyCoroutine());

        if (roundCnt > 3)
            ChangeTurnToNextPlayer();
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

    // ���� ���� �� UI�� ������ִ� �ڷ�ƾ
    private IEnumerator ShowGameEndUICoroutine()
    {
        yield return new WaitForSeconds(2.0f);

        // ���� ���� UI ���
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
}
