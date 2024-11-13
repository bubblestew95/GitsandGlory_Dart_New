using UnityEngine;

public class DartPlayer : MonoBehaviour
{
    public int totalScore = 0;  // �÷��̾��� �� ����
    public int attemptCount = 0;  // ���� ������ Ƚ��

    // ������ �Լ�
    public void ThrowDart(int _score)
    {
        if (attemptCount < 3)
        {
            ++attemptCount;  // ������ Ƚ�� ����
            totalScore += _score;  // �����⿡�� ���� ���� �߰�
            DartGameManager.Instance.UpdatePlayerScore(_score);  // UI ������Ʈ ��û
        }

        if (attemptCount == 0)
        {
            DartGameManager.Instance.EndTurn();  // �����Ⱑ ������ �� ����
        }
    }
}
