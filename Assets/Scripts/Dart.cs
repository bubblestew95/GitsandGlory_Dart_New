using System.Collections;
using UnityEngine;

using Photon.Pun;

public class Dart : MonoBehaviourPun
{
    // 다트의 시작점에서 끝점까지 움직이는 데 걸리는 시간
    [SerializeField]
    private float arrivedTime = 0.5f;

    [SerializeField]
    private AudioClip throwSound = null;

    private Vector3 dir = Vector3.zero;
    private Vector3 startPos = Vector3.zero;
    private Vector3 endPos = Vector3.zero;

    public Vector2 EndPosition
    {
        get { return new Vector2(endPos.x, endPos.y);}
    }

    // startPos ~ endPos 로 일직선 발사 시작
    // 추후에 포물선 발사 구현 가능?
    public void ThrowDart(Vector3 _startPos, Vector3 _endPos)
    {
        photonView.RPC("PlayThrowAudio", RpcTarget.All);

        dir = (_endPos - _startPos).normalized;
        startPos = _startPos;
        endPos = _endPos;

        StartCoroutine(DartFlyingCoroutine());
    }

    // 다트 발사 중 이동 코루틴
    private IEnumerator DartFlyingCoroutine()
    {
        Vector3 center = (startPos + endPos) * 0.5f;
        center.y -= 20;

        Vector3 _start = startPos - center;
        Vector3 _end = endPos - center;

        Quaternion startQuat = Quaternion.Euler(-45f, 0f, 0f);
        Quaternion endQuat = Quaternion.Euler(45f, 0f, 0f);

        float ratio = 0f;

        while (ratio <= 1f)
        {
            transform.position = Vector3.Slerp(_start, _end, ratio) + center;

            transform.rotation = Quaternion.Slerp(startQuat, endQuat, ratio);

            ratio += Time.deltaTime / arrivedTime;

            yield return null;
        }
    }

    [PunRPC]
    private void PlayThrowAudio()
    {
        if(throwSound)
        {
            GameManager.Instance.AudioSrc.PlayOneShot(throwSound);
        }

    }
}
