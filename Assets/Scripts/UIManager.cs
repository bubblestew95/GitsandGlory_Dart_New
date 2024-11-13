using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text Round1;  // 플레이어 점수를 표시할 텍스트 UI
    public Text Round2;  // 플레이어 점수를 표시할 텍스트 UI
    public Text Round3;  // 플레이어 점수를 표시할 텍스트 UI
    public Text total;
    public DartPlayer player;

    // 점수를 UI에 업데이트하는 함수
    public void UpdateScore(int score)
    {
        switch (player.attemptCount)
        {
            case 1:
                Round1.text = "Score: " + score.ToString();
                break;
            case 2:
                Round2.text = "Score: " + score.ToString();
                break;
            case 3:
                Round3.text = "Score: " + score.ToString();
                break;
        }
        total.text = "totalScore" + player.totalScore.ToString();
    }
}
