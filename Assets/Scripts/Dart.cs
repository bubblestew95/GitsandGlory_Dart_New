using System.Collections;
using UnityEngine;

using Photon.Pun;

public class Dart : MonoBehaviourPun
{
    // ��Ʈ�� ���������� �������� �����̴� �� �ɸ��� �ð�
    [SerializeField]
    private float arrivedTime = 1f;

    // ������ ������ �ִ� ����
    [SerializeField]
    private float parabolaMaxHeight = 2f;

    [SerializeField]
    private AudioClip throwSound = null;

    [SerializeField]
    private AudioClip hitSound = null;

    private Vector3 dir = Vector3.zero;
    private Vector3 startPos = Vector3.zero;
    private Vector3 endPos = Vector3.zero;

    public Vector2 EndPosition
    {
        get { return new Vector2(endPos.x, endPos.y);}
    }

    // startPos ~ endPos �� ������ �߻� ����
    // ���Ŀ� ������ �߻� ���� ����?
    public void ThrowDart(Vector3 _startPos, Vector3 _endPos)
    {
        photonView.RPC("PlayThrowAudio", RpcTarget.All);

        dir = (_endPos - _startPos).normalized;
        startPos = _startPos;
        endPos = _endPos;

        StartCoroutine(DartFlyingCoroutine());
    }

    // ��Ʈ �߻� �� �̵� �ڷ�ƾ
    private IEnumerator DartFlyingCoroutine()
    {
        float ratio = 0f;

        while (ratio <= arrivedTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, ratio);

            ratio += Time.deltaTime / arrivedTime;

            yield return null;
        }

        photonView.RPC("PlayHitAudio", RpcTarget.All);
    }

    [PunRPC]
    private void PlayThrowAudio()
    {
        if(throwSound)
        {
            // Bgm ������ �̰�
            GameManager.Instance.AudioSrc.PlayOneShot(throwSound);
        }

    }

    [PunRPC]
    private void PlayHitAudio()
    {
        if(hitSound)
        {
            // Bgm ������ �̰�
            GameManager.Instance.AudioSrc.PlayOneShot(hitSound);
        }
    }
}
