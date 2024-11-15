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