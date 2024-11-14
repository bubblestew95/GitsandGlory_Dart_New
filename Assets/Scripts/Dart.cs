using System.Collections;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    private float dartBoardZPos = 0f;

    private Vector3 dir = Vector3.zero;

    // 다트 판까지의 거리 측정을 위한 디버깅 스피어 그리기
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector3(0f, 0f, dartBoardZPos), 2f);
    }

    // startPos ~ endPos 로 일직선 발사 시작
    // 추후에 포물선 발사 구현 가능?
    public void ThrowDart(Vector3 _startPos, Vector3 _endPos)
    {
        dir = (_endPos - _startPos).normalized;

        StartCoroutine(DartFlyingCoroutine());
    }

    // 다트 발사 중 이동 코루틴
    private IEnumerator DartFlyingCoroutine()
    {
        while(transform.position.z < dartBoardZPos)
        {
            transform.Translate(dir * speed * Time.deltaTime);

            yield return null;
        }
    }
}
