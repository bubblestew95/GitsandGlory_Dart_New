using UnityEngine;

public class ScoreEvaluate : MonoBehaviour
{

    private Vector2 coordinate = Vector2.zero;

    private float length = 0;
    private float angle = 0;
    private float CalculateLength(float x, float y) => Mathf.Sqrt(x * x + y * y);

    private float CalculateTheta(float x, float y) => Mathf.Atan2(y, x);


    private int ScoreDecision(int _scoreNumber)
    {
        int score = 0;
        length = CalculateLength(coordinate.x, coordinate.y);

        // length ���� ���� ���� ���̺�
        if (length > 0 && length <= 1)
            score = 50;
        else if (length > 1 && length <= 2)
            score = 30;
        else if (length > 2 && length <= 5)
            score = _scoreNumber;
        else if (length > 5 && length <= 6)
            score = _scoreNumber * 2;
        else if (length > 7 && length <= 9)
            score = _scoreNumber;
        else if (length > 9 && length <= 10)
            score = _scoreNumber * 3;

        return score;
    }


    private int ScoreNumber()
    {
        int rate = 9;
        int scoreNumber = 0;

        // angle ���
        angle = CalculateTheta(coordinate.x, coordinate.y);

        // �� ������ �ش��ϴ� ������ �迭�� ����
        int[] scoreTable = new int[] { 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8, 11, 14, 9, 12, 5 };

        // angle�� �ش��ϴ� �ε����� ��� (rate �������� ������)
        int index = (int)(angle / rate);  // angle�� rate�� ���� ���� �ε����� ���

        // �ε����� ������ ����� �ʵ��� Ŭ����
        if (index < 0) index = 0;
        if (index >= scoreTable.Length) index = scoreTable.Length - 1;

        scoreNumber = scoreTable[index];

        return scoreNumber;
    }



}
