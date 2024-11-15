using System.Collections;
using UnityEngine;

using Photon.Pun;

public class Dart : MonoBehaviourPun
{
    // 다트의 시작점에서 끝점까지 움직이는 데 걸리는 시간
    [SerializeField]
    private float arrivedTime = 1f;

    // 포물선 궤적의 최대 높이
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
            // Bgm 없으면 이거
            GameManager.Instance.AudioSrc.PlayOneShot(throwSound);
        }

    }

    [PunRPC]
    private void PlayHitAudio()
    {
        if(hitSound)
        {
            // Bgm 없으면 이거
            GameManager.Instance.AudioSrc.PlayOneShot(hitSound);
        }
    }
}
