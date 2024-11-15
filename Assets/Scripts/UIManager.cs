using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI currentRound;
    public TextMeshProUGUI round1;  // �÷��̾� ������ ǥ���� �ؽ�Ʈ UI
    public TextMeshProUGUI round2;  // �÷��̾� ������ ǥ���� �ؽ�Ʈ UI
    public TextMeshProUGUI round3;  // �÷��̾� ������ ǥ���� �ؽ�Ʈ UI
    public TextMeshProUGUI total;
    public TextMeshProUGUI playerName;
    public DartPlayer player;

    //������ UI�� ������Ʈ�ϴ� �Լ�
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




    // ���� �迭�� ����
    private int[] scores = new int[3];

    // ���� ���� �Լ�
    public void UpdateScoreUI(int[] newScores)
    {
        int i;
        // ���ο� ���� �迭�� �޾Ƽ� ����
        for (i = 0; i < 3; ++i)
        {
            scores[i] = newScores[i];
        }

        currentRound.text = "Round : " + i.ToString();
        // �� TextMeshProUGUI �ؽ�Ʈ�� ���� ǥ��
        round1.text = "Score 1: " + scores[0];
        round2.text = "Score 2: " + scores[1];
        round3.text = "Score 3: " + scores[2];

        // ���� ��� �� ���
        int totalScore = scores[0] + scores[1] + scores[2];
        total.text = "Total Score: " + totalScore.ToString();
    }

}
