using UnityEngine;

using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviourPun
{
    private Vector3 dartStartPos = Vector3.zero;
    public int PlayerActorNum
    {
        get; set;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
        }
        // ������ ���� ���¸� ����
        if (GameManager.Instance.IsGameOver) 
            return;

        // ���� Ŭ���̾�Ʈ�� ���� ��Ʈ�ѷ���� ����
        if (!photonView.IsMine)
            return;

        // �� �÷��̾��� ���� �ƴ϶�� ����
        if (photonView.OwnerActorNr != GameManager.Instance.CurrentTurnPlayerActorNum)
            return;

        // ���� ���尡 �غ���� �ʾҴٸ� ����
        if (!GameManager.Instance.IsNextRoundReady)
            return;

        // ���콺 �� �� ��ư Ŭ�� �� ��Ʈ�� ������.
        if (Input.GetMouseButtonDown(0))
        {
            {
                dartStartPos = GameManager.Instance.PointerUI.GetPointerWorldPos();
                dartStartPos.z = -25f;
            }

            photonView.RPC("SetDartStartPos", RpcTarget.Others, dartStartPos);

            // ��Ʈ�� ������
            Vector3 dartEndPos = new Vector3(dartStartPos.x, dartStartPos.y, 0f);

            // ��Ʈ�� �������� �������� ������ �����.
            int score = GameManager.Instance.ScoreEvaluate.EvaluateScore(dartEndPos);

            // ��Ʈ��ũ�� ���ؼ� ��� ������Ʈ���� ��Ʈ �����ϵ��� ���.
            GameObject dartGo = PhotonNetwork.Instantiate("P_Dart", dartStartPos, Quaternion.identity);

            // ��Ʈ ��ô
            dartGo.GetComponent<Dart>().ThrowDart(dartStartPos, dartEndPos);

            // ���������� ���� ��Ʈ�� �������� ����
            GameManager.Instance.photonView.RPC("UpdateLastDartEndPoint", RpcTarget.All, dartEndPos);

            // ��Ʈ�� ������ �̹� ���尡 �����ٰ� ���� �Ŵ������� �˸�.
            GameManager.Instance.RoundComplete(score);
        }
    }

    [PunRPC]
    private void SetDartStartPos(Vector3 _pos)
    {
        dartStartPos = _pos;
    }
}
