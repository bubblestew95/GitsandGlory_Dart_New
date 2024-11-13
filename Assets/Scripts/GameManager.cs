using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region CallbackFunctions
    public override void OnPlayerLeftRoom(Player _otherPlayer)
    {
        Debug.LogFormat("Player Left Room : {0}", _otherPlayer.NickName);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");

        // Todo : �κ� ������ �ٽ� ���ư���.
        // SceneManager.LoadScene("");
    }
    #endregion

    public void LeaveRoom()
    {
        Debug.Log("Leave Room");

        PhotonNetwork.LeaveRoom();
    }
}
