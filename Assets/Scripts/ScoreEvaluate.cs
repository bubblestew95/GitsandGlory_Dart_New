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

        // length 값에 따른 점수 테이블
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

        // angle 계산
        angle = CalculateTheta(coordinate.x, coordinate.y);

        // 각 범위에 해당하는 점수를 배열로 정의
        int[] scoreTable = new int[] { 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8, 11, 14, 9, 12, 5 };

        // angle에 해당하는 인덱스를 계산 (rate 간격으로 나누기)
        int index = (int)(angle / rate);  // angle을 rate로 나눈 몫을 인덱스로 사용

        // 인덱스가 범위를 벗어나지 않도록 클램핑
        if (index < 0) index = 0;
        if (index >= scoreTable.Length) index = scoreTable.Length - 1;

        scoreNumber = scoreTable[index];

        return scoreNumber;
    }



}
