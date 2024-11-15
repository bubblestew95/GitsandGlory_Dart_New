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
        center.y -= 10;
        startPos -= center;
        endPos -= center;

        
        float ratio = 0f;

        while (ratio <= arrivedTime)
        {
            //transform.position = Vector3.Lerp(startPos, endPos, ratio);
            transform.position = Vector3.Slerp(startPos, endPos, ratio);
            transform.position += center;

            ratio += Time.deltaTime / arrivedTime;

            yield return null;
        }
    }
}
