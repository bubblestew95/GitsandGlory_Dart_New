using System.Collections;
using UnityEngine;

using Photon.Pun;

public class Dart : MonoBehaviourPun
{
    // ��Ʈ�� ���������� �������� �����̴� �� �ɸ��� �ð�
    [SerializeField]
    private float arrivedTime = 0.5f;

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
        //float gravity = 9.8f;
        //float v0 = 11.0f;
        //Vector3 curPos = Vector3.zero;

        Vector3 center = (startPos + endPos) * 0.5f;
        center.y -= 20;

        Vector3 _start = startPos - center;
        Vector3 _end = endPos - center;

        float ratio = 0f;

        while (ratio <= 1f)
        {
            //transform.position = Vector3.Lerp(startPos, endPos, ratio);
            Vector3 nextPos = Vector3.Slerp(_start, _end, ratio) + center;

            //Vector3 dir = (nextPos - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.Euler(-transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z),
                                                  ratio);

            transform.position = nextPos;

            // transform.rotation = Quaternion.LookRotation(dir);

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
