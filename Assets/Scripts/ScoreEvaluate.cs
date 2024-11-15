using UnityEngine;

public class ScoreEvaluate
{
    private Vector2 coordinate = Vector2.zero;

    private float length = 0;
    private float angle = 0;
    private float CalculateLength(float x, float y) => Mathf.Sqrt((x * x) + (y * y));
    //음수 없이 세팅, degree출력
    private float CalculateAngle(float x, float y) => (Mathf.Atan2(x, y) * Mathf.Rad2Deg + 360) % 360;

    private int ScoreDecision(int _scoreNumber, Vector2 _coordinate)
    {
        int score = 0;
        length = CalculateLength(_coordinate.x, _coordinate.y);
        //Debug.Log("length: " + length);

        // length 값에 따른 점수 테이블
        if (length > 0 && length <= 0.42)
            score = 50;
        else if (length > 0.42 && length <= 0.9)
            score = 30;
        else if (length > 0.9 && length <= 5.75)
            score = _scoreNumber;
        else if (length > 5.75 && length <= 6.64)
            score = _scoreNumber * 2;
        else if (length > 6.64 && length <= 9.08)
            score = _scoreNumber;
        else if (length > 9.08 && length <= 10)
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

        // angle 계산
        angle = CalculateAngle(_coordinate.x, _coordinate.y);
        //Debug.Log("angle: " + angle);
        // 각 범위에 해당하는 점수를 배열로 정의
        int[] scoreTable = new int[] { 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8, 11, 14, 9, 12, 5 };

        // angle에 해당하는 인덱스를 계산 (rate 간격으로 나누기)
        int index = (int)((angle + 9) / rate);  // angle을 rate로 나눈 몫을 인덱스로 사용

        // 인덱스가 범위를 벗어나지 않도록 클램핑
        if ((index < 0) || (index >= scoreTable.Length)) index = 0;

        scoreNumber = scoreTable[index];

        return scoreNumber;
    }

    /// <summary>
    /// 2차원 벡터를 받아서 다트판에서의 점수를 판정함.
    /// </summary>
    /// <param name="_coordinate">점수를 판정할 포지션의 x,y 벡터</param>
    /// <returns></returns>
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
