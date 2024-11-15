using UnityEngine;

using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{

    private Dart throwedDart = null;

    private Vector3 dartStartPos = Vector3.zero;
    public int PlayerActorNum
    {
        get; set;
    }

    private void Update()
    {
        // 게임이 끝난 상태면 리턴
        if (GameManager.Instance.IsGameOver) 
            return;

        // 원격 클라이언트의 게임 컨트롤러라면 리턴
        if (!photonView.IsMine)
            return;

        // 이 플레이어의 턴이 아니라면 리턴
        if (photonView.OwnerActorNr != GameManager.Instance.CurrentTurnPlayerActorNum)
            return;

        // 다음 라운드가 준비되지 않았다면 리턴
        if (!GameManager.Instance.IsNextRoundReady)
            return;

        // 마우스 왼 쪽 버튼 클릭 시 다트를 던진다.
        if (Input.GetMouseButtonDown(0))
        {
            {
                dartStartPos = GameManager.Instance.PointerUI.GetPointerWorldPos();
                dartStartPos.z = -25f;
            }

            photonView.RPC("SetDartStartPos", RpcTarget.Others, dartStartPos);

            // 다트의 도착점
            Vector3 dartEndPos = new Vector3(dartStartPos.x, dartStartPos.y, 0f);

            // 다트의 도착점을 기준으로 점수를 계산함.
            int score = GameManager.Instance.ScoreEvaluate.EvaluateScore(dartEndPos);

            // 네트워크를 통해서 모든 오브젝트에게 다트 생성하도록 명령.
            GameObject dartGo = PhotonNetwork.Instantiate("P_Dart", dartStartPos, Quaternion.Euler(-89.9f, 0f, 0f));

            // 다트 투척
            dartGo.GetComponent<Dart>().ThrowDart(dartStartPos, dartEndPos);

            // 마지막으로 던진 다트의 도착점을 갱신
            GameManager.Instance.photonView.RPC("UpdateLastDartEndPoint", RpcTarget.All, dartEndPos);

            // 다트를 던지면 이번 라운드가 끝났다고 게임 매니저에게 알림.
            GameManager.Instance.RoundComplete(score);
        }
    }

    [PunRPC]
    private void SetDartStartPos(Vector3 _pos)
    {
        dartStartPos = _pos;
    }

    //마지막에 던진 다트의 도착점 좌표
    //public Vector2 GetLastDartEndPos()
    //{
    //    return lastThrowedDart.EndPosition;
    //}
}
