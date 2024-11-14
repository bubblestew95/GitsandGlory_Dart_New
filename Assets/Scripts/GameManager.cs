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
    // �ٽ� �κ�� ���ư��� ��ư
    [SerializeField]
    private Button backToLobbyBtn = null;

    // ��Ʈ ������. �̱ۿ����� �׽�Ʈ�� ������ �ʿ� ����.
    [SerializeField]
    private GameObject dartPrefab = null;

    // ���� �� ��
    private int turnCnt = 1;
    // �̹� �Ͽ��� ���� ��Ʈ ��
    private int roundCnt = 1;
    // ���� ���� �÷��̾� Ű��.
    private int curTurnPlayerKeyValue = 1;

    private bool isGameOver = false;

    #region CallbackFunctions
    private void Start()
    {
        // Todo : ���� ���� ������ ���� ������ �г��� UI�� ������Ʈ�Ѵ�.
    }

    private void Update()
    {
        // ������ ���� ���¸� ����
        if (isGameOver) return;

        // ���� Ŭ���̾�Ʈ�� ���� �Ŵ������ ����
        if (!photonView.IsMine)
            return;

        // �� Ŭ���̾�Ʈ�� �÷��̾��� ���� �ƴ϶�� ����
        if (photonView.Owner.ActorNumber != 
            PhotonNetwork.CurrentRoom.Players[curTurnPlayerKeyValue].ActorNumber)
        {
            return;
        }

        // ���콺 �� �� ��ư Ŭ�� �� ��Ʈ�� ������.
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
            }// ��ũ�� �� ���콺 �������� ��Ʈ ��ô ���������� ����. ���� ������ UI ���� �� �ش� ��ġ�� ������Ʈ ����.

            ThrowDart(dartThrowPos);

            // ��Ʈ�� ������ ���带 1 �ø���.
            ++roundCnt;

            // 3���尡 �����ٸ� ���� �Ѿ��.
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

    // ���� ������ �Լ�
    public void LeaveRoom()
    {
        Debug.Log("Leave Room");

        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// ��Ʈ ������Ʈ�� ���� �� Dart ��ũ��Ʈ�� ThrowDart �޼ҵ带 ȣ���Ѵ�.
    /// </summary>
    /// <param name="_spawnPos">��Ʈ�� ���� ����.</param>
    public void ThrowDart(Vector3 _spawnPos)
    {
        // GameObject dartGo = PhotonNetwork.Instantiate("P_Dart", _spawnPos, Quaternion.identity);
        GameObject dartGo = Instantiate<GameObject>(dartPrefab, _spawnPos, Quaternion.identity);
        Dart dart = dartGo.GetComponent<Dart>();

        dart.ThrowDart(_spawnPos, new Vector3(_spawnPos.x, _spawnPos.y, 0f));
    }

    // ���� ���� �÷��̾�� �ѱ��.
    [PunRPC]
    public void ChangeTurnToNextPlayer()
    {
        ++turnCnt;

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
    }

    // ���� ��. ���� ��� ��Ȱ��ȭ�ϰ� ������ ����Ѵ�.
    private void EndGame()
    {
        Debug.Log("Game End!");

        // ���� ����� ��Ȱ��ȭ�Ѵ�.
        isGameOver = true;

        // �ణ�� �ð� �ڿ� ���� UI�� ����Ѵ�.

    }
}
