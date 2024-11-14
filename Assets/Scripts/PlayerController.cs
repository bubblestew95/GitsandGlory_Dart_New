using UnityEngine;

using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public int PlayerActorNum
    {
        get; set;
    }

    private void Update()
    {
        // ������ ���� ���¸� ����
        if (GameManager.Instance.IsGameOver) return;

        // ���� Ŭ���̾�Ʈ�� ���� ��Ʈ�ѷ���� ����
        if (!photonView.IsMine)
            return;

        // �� �÷��̾��� ���� �ƴ϶�� ����
        if (photonView.OwnerActorNr != GameManager.Instance.CurrentTurnPlayerActorNum)
        {
            return;
        }

        // ���� ���尡 �غ���� �ʾҴٸ� ����
        if (!GameManager.Instance.IsNextRoundReady)
            return;

        // ���콺 �� �� ��ư Ŭ�� �� ��Ʈ�� ������.
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 dartThrowPos = Vector3.zero;

            {
                dartThrowPos = Camera.main.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y,
                    -Camera.main.transform.position.z)
                    );
                dartThrowPos.z = -20f;
            }// ��ũ�� �� ���콺 �������� ��Ʈ ��ô ���������� ����. ���� ������ UI ���� �� �ش� ��ġ�� ������Ʈ ����.

            // ��Ʈ��ũ�� ���ؼ� ��� ������Ʈ���� ��Ʈ �����ϵ��� ���.
            GameObject dartGo = PhotonNetwork.Instantiate("P_Dart", Vector3.zero, Quaternion.identity);

            photonView.RPC("ThrowDarte", RpcTarget.All, dartGo.GetComponent<Dart>(), dartThrowPos);

            // ��Ʈ�� ������ ���带 �����ߴٰ� ���� �Ŵ������� �˸�.
            GameManager.Instance.RoundComplete();
        }
    }

    // <summary>
    // ��Ʈ ������Ʈ�� ���� �� Dart ��ũ��Ʈ�� ThrowDart �޼ҵ带 ȣ���Ѵ�.
    // </summary>
    // <param name = "_spawnPos" > ��Ʈ�� ���� ����.</param>
    [PunRPC]
    public void ThrowDarte(Dart _dart, Vector3 _spawnPos)
    {
        _dart.ThrowDart(_spawnPos, new Vector3(_spawnPos.x, _spawnPos.y, 0f));
    }

    //�������� ���� ��Ʈ�� ������ ��ǥ
    //public Vector2 GetLastDartEndPos()
    //{
    //    return lastThrowedDart.EndPosition;
    //}
}
