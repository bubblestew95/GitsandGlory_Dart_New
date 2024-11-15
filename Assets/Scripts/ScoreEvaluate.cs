using UnityEngine;

public class ScoreEvaluate
{

    private Vector2 coordinate = Vector2.zero;

    private float length = 0;
    private float angle = 0;
    private float CalculateLength(float x, float y) => Mathf.Sqrt((x * x) + (y * y));

    private float CalculateAngle(float x, float y) => (Mathf.Atan2(x, y) * Mathf.Rad2Deg + 360) % 360;


    private int ScoreDecision(int _scoreNumber, Vector2 _coordinate)
    {
        int score = 0;
        length = CalculateLength(_coordinate.x, _coordinate.y);
        //Debug.Log("length: " + length);

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
        else
            score = 0;

        //Debug.Log("number: " +_scoreNumber + " -- " + "score: "+ score);
        return score;
    }


    private int ScoreNumber(Vector2 _coordinate)
    {
        int rate = 18;
        int scoreNumber = 0;

        // angle ���
        angle = CalculateAngle(_coordinate.x, _coordinate.y);
        //Debug.Log("angle: " + angle);
        // �� ������ �ش��ϴ� ������ �迭�� ����
        int[] scoreTable = new int[] { 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8, 11, 14, 9, 12, 5 };

        // angle�� �ش��ϴ� �ε����� ��� (rate �������� ������)
        int index = (int)((angle + 9) / rate);  // angle�� rate�� ���� ���� �ε����� ���

        // �ε����� ������ ����� �ʵ��� Ŭ����
        if ((index < 0) || (index >= scoreTable.Length)) index = 0;

        scoreNumber = scoreTable[index];

        return scoreNumber;
    }

    public int EvaluateScore(Vector2 _coordinate)
    {
        if (coordinate == null)
        {
            Debug.LogError("Coordinate is null.");
            return 0;
        }

        int scoreNumber = ScoreNumber(_coordinate);
        int score = ScoreDecision(scoreNumber, _coordinate);
        Debug.Log(score);
        return score;
    }
}
