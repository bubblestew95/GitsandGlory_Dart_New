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

    [SerializeField]
    private GameObject dartPrefab = null;

    // ���� �� ��
    private int turnCnt = 1;
    // ���� ���� �÷��̾� Ű��.
    private int curTurnPlayerKeyValue = 1;

    #region CallbackFunctions
    private void Start()
    {
        // Todo : ���� ���� ������ ���� ������ �г��� UI�� ������Ʈ�Ѵ�.
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, 
                -Camera.main.transform.position.z)
                );

            mousePos.z = -25f;

            ThrowDart(mousePos);
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

    // ��Ʈ�� ������.
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
