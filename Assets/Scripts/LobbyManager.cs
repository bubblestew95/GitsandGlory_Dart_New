using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class LobbyManager : MonoBehaviourPun
{
    // 로비 -> 게임 대기실로 들어가는 버튼
    [SerializeField]
    private Button gameRoomEnterBtn = null;

    // 마스터가 게임 대기실 -> 게임 시작하는 버튼
    [SerializeField]
    private Button gameStartBtn = null;

    // 한 방의 최대 플레이어 수
    private int maxPlayerPerRoom = 4;

    // 게임 버전
    private string gameVersion = "0.0.1";
}
