using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region CallbackFunctions
    private void Start()
    {
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

    public void LeaveRoom()
    {
        Debug.Log("Leave Room");

        PhotonNetwork.LeaveRoom();
    }
}
