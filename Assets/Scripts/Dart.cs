using System.Collections;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    private float dartBoardZPos = 0f;

    private Vector3 dir = Vector3.zero;

    // ��Ʈ �Ǳ����� �Ÿ� ������ ���� ����� ���Ǿ� �׸���
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector3(0f, 0f, dartBoardZPos), 2f);
    }

    // startPos ~ endPos �� ������ �߻� ����
    // ���Ŀ� ������ �߻� ���� ����?
    public void ThrowDart(Vector3 _startPos, Vector3 _endPos)
    {
        dir = (_endPos - _startPos).normalized;

        StartCoroutine(DartFlyingCoroutine());
    }

    // ��Ʈ �߻� �� �̵� �ڷ�ƾ
    private IEnumerator DartFlyingCoroutine()
    {
        while(transform.position.z < dartBoardZPos)
        {
            transform.Translate(dir * speed * Time.deltaTime);

            yield return null;
        }
    }
}
