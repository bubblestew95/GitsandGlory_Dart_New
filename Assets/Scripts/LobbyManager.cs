using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class LobbyManager : MonoBehaviourPun
{
    // �κ� -> ���� ���Ƿ� ���� ��ư
    [SerializeField]
    private Button gameRoomEnterBtn = null;

    // �����Ͱ� ���� ���� -> ���� �����ϴ� ��ư
    [SerializeField]
    private Button gameStartBtn = null;

    // �� ���� �ִ� �÷��̾� ��
    private int maxPlayerPerRoom = 4;

    // ���� ����
    private string gameVersion = "0.0.1";
}
