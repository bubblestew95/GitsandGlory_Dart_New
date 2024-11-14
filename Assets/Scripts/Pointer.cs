using System.Collections;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTr = null;

    private Vector2 resetPos = new Vector2(-10f, -10f);


    private void Start()
    {
        InvokeRepeating("DrawPointerCoroutine", 0f, 0.1f);
    }


    private void Update()
    {

    }

    private void DrawPointerCoroutine()
    {
        Vector3 mousePos = Input.mousePosition;

        Vector3 mouseScreenToWorld =
            Camera.main.ScreenToWorldPoint(mousePos);
        Ray MousePosToRay =
            Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(MousePosToRay, out hit))
        {
            rectTr.position = new Vector2(
                mousePos.x + Random.Range(-10f, 10f),
                mousePos.y + Random.Range(-10f, 10f));
        }
        else
        {
            rectTr.position = resetPos;
        }
    }
}