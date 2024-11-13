using UnityEngine;

public class DartPlayer : MonoBehaviour
{
    public int totalScore = 0;  // 플레이어의 총 점수
    public int attemptCount = 0;  // 남은 던지기 횟수

    // 던지기 함수
    public void ThrowDart(int _score)
    {
        if (attemptCount < 3)
        {
            ++attemptCount;  // 던지기 횟수 차감
            totalScore += _score;  // 던지기에서 나온 점수 추가
            DartGameManager.Instance.UpdatePlayerScore(_score);  // UI 업데이트 요청
        }

        if (attemptCount == 0)
        {
            DartGameManager.Instance.EndTurn();  // 던지기가 끝나면 턴 종료
        }
    }
}
