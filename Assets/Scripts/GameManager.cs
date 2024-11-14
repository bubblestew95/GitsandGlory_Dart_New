using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    // �ٽ� �κ�� ���ư��� ��ư
    [SerializeField]
    private Button backToLobbyBtn = null;

    // ���� �� ��
    private int turnCnt = 1;
    // ���� ���� �÷��̾� Ű��.
    private int curTurnPlayerKeyValue = 1;

    #region CallbackFunctions
    private void Start()
    {
        // Todo : ���� ���� ������ ���� ������ �г��� UI�� ������Ʈ�Ѵ�.
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
        // ���� ����� ��Ȱ��ȭ�Ѵ�.

        // �ణ�� �ð� �ڿ� ���� UI�� ����Ѵ�.

    }
}
