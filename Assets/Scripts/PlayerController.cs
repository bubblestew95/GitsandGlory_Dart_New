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
                dartStartPos = Camera.main.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y,
                    -Camera.main.transform.position.z)
                    );
                dartStartPos.z = -20f;
            }// ��ũ�� �� ���콺 �������� ��Ʈ ��ô ���������� ����. ���� ������ UI ���� �� �ش� ��ġ�� ������Ʈ ����.

            photonView.RPC("SetDartStartPos", RpcTarget.Others, dartStartPos);

            // ��Ʈ�� ������
            Vector3 dartEndPos = new Vector3(dartStartPos.x, dartStartPos.y, 0f);

            // ��Ʈ��ũ�� ���ؼ� ��� ������Ʈ���� ��Ʈ �����ϵ��� ���.
            GameObject dartGo = PhotonNetwork.Instantiate("P_Dart", dartStartPos, Quaternion.identity);

            // ��Ʈ ��ô
            dartGo.GetComponent<Dart>().ThrowDart(dartStartPos, dartEndPos);
            // photonView.RPC("PlayerThrowDart", RpcTarget.All, dartThrowPos);

            // ���������� ���� ��Ʈ�� �������� ����
            GameManager.Instance.photonView.RPC("UpdateLastDartEndPoint", RpcTarget.All, dartEndPos);

            // ��Ʈ�� ������ ���带 �����ߴٰ� ���� �Ŵ������� �˸�.
            GameManager.Instance.RoundComplete();
        }
    }

    [PunRPC]
    private void SetDartStartPos(Vector3 _pos)
    {
        dartStartPos = _pos;
    }

    //�������� ���� ��Ʈ�� ������ ��ǥ
    //public Vector2 GetLastDartEndPos()
    //{
    //    return lastThrowedDart.EndPosition;
    //}
}
