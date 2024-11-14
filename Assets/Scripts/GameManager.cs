using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button gameExitBtn = null;

    // ���� ��
    private int turnCnt = 1;
    // ���� ���� �÷��̾� Ű��.
    private int curTurnPlayerNum = 1;

    #region CallbackFunctions
    private void Start()
    {
        // Todo : ���� �÷��̾��� ������ ���� ������ UI�� ������Ʈ�Ѵ�. (�г���...)
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

    // ���� ���� �÷��̾�� �ѱ��.
    [PunRPC]
    public void ChangeTurnToNextPlayer()
    {
        ++turnCnt;

        // ���� ���� ���� �÷��̾��� ������ �� ���ٸ� �÷��̾��� ����ŭ ���� ���Ҵٴ� ��
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
        // ���� ����� ��Ȱ��ȭ�Ѵ�.

        // �ణ�� �ð� �ڿ� ���� UI�� ����Ѵ�.

    }
}
