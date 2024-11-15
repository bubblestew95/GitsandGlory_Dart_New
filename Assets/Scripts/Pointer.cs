using System.Collections;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private RectTransform rectTr = null;

    private Vector3 cursorPos;   // ���콺 Ŀ�� ��ġ
    private Vector3 pointerPos;  // ���� ������ ��ġ
    private Vector3 resetPos; // Ŀ���� ������Ʈ ��� �� ���콺 ���� ��ġ

    public float pointerRndRng = 80f;  // ��鸲 ����
    public float moveSpeed = 8f;      // �̵� �ӵ� ����

    private void Start()
    {
        pointerPos = resetPos;  // ������ ��ġ �ʱ�ȭ
        rectTr = GetComponent<RectTransform>();
    }

    private void Update()
    {
        DrawPointer();
    }

    public Vector3 GetPointerWorldPos()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(
            new Vector3(
                rectTr.position.x,
                rectTr.position.y,
                -Camera.main.transform.position.z)
            );

        // Debug.LogFormat("Rect Pos : {0}, {1}   World Pos : {2}, {3}", rectTr.position.x, rectTr.position.y, pos.x, pos.y);

        return pos;
    }

    private void DrawPointer()
    {
        resetPos = new Vector3(-10f, -10f, 0f);
        cursorPos = Input.mousePosition;
        Ray cursorPosToRay =
            Camera.main.ScreenPointToRay(cursorPos);
        RaycastHit hit;


        if (Physics.Raycast(cursorPosToRay, out hit))
        {
            pointerPos = cursorPos + new Vector3(
                Random.Range(-pointerRndRng, pointerRndRng),
                Random.Range(-pointerRndRng, pointerRndRng),
                0);

            // Lerp�� �̿��Ͽ� �����Ϳ��� ������ ���� ������ ����
            rectTr.position = Vector3.Lerp(rectTr.position, pointerPos, Time.deltaTime * moveSpeed);
        }
        else
        {
            rectTr.position = resetPos;
        }
    }
}