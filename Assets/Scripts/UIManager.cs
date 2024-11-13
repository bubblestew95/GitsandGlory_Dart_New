using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text Round1;  // �÷��̾� ������ ǥ���� �ؽ�Ʈ UI
    public Text Round2;  // �÷��̾� ������ ǥ���� �ؽ�Ʈ UI
    public Text Round3;  // �÷��̾� ������ ǥ���� �ؽ�Ʈ UI
    public Text total;
    public DartPlayer player;

    // ������ UI�� ������Ʈ�ϴ� �Լ�
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
