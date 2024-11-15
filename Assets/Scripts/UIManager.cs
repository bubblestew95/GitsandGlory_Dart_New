using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI currentRound;
    public TextMeshProUGUI round1;  // 플레이어 점수를 표시할 텍스트 UI
    public TextMeshProUGUI round2;  // 플레이어 점수를 표시할 텍스트 UI
    public TextMeshProUGUI round3;  // 플레이어 점수를 표시할 텍스트 UI
    public TextMeshProUGUI total;
    public TextMeshProUGUI playerName;
    public DartPlayer player;

    //점수를 UI에 업데이트하는 함수
    public void UpdateScore(int score)
    {
        switch (player.attemptCount)
        {
            case 1:
                round1.text = "Score: " + score.ToString();
                break;
            case 2:
                round2.text = "Score: " + score.ToString();
                break;
            case 3:
                round3.text = "Score: " + score.ToString();
                break;
        }
        total.text = "totalScore" + player.totalScore.ToString();
    }




    // 점수 배열을 관리
    private int[] scores = new int[3];

    // 점수 갱신 함수
    public void UpdateScoreUI(int[] newScores)
    {
        int i;
        // 새로운 점수 배열을 받아서 갱신
        for (i = 0; i < 3; ++i)
        {
            scores[i] = newScores[i];
        }

        currentRound.text = "Round : " + i.ToString();
        // 각 TextMeshProUGUI 텍스트에 점수 표시
        round1.text = "Score 1: " + scores[0];
        round2.text = "Score 2: " + scores[1];
        round3.text = "Score 3: " + scores[2];

        // 총합 계산 및 출력
        int totalScore = scores[0] + scores[1] + scores[2];
        total.text = "Total Score: " + totalScore.ToString();
    }

}
