using System.Collections;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private RectTransform rectTr = null;

    private Vector3 cursorPos;   // 마우스 커서 위치
    private Vector3 pointerPos;  // 랜덤 포인터 위치
    private Vector3 resetPos; // 커서가 오브젝트 벗어날 시 마우스 리셋 위치

    public float pointerRndRng = 80f;  // 흔들림 범위
    public float moveSpeed = 8f;      // 이동 속도 조절

    private void Start()
    {
        pointerPos = resetPos;  // 포인터 위치 초기화
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

            // Lerp를 이용하여 포인터에서 포인터 사이 움직임 보간
            rectTr.position = Vector3.Lerp(rectTr.position, pointerPos, Time.deltaTime * moveSpeed);
        }
        else
        {
            rectTr.position = resetPos;
        }
    }
}