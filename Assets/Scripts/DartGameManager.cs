using UnityEngine;

public class DartGameManager : MonoBehaviour
{
    public static DartGameManager Instance;
    public DartPlayer player;  // �÷��̾� ����
    public UIManager uiManager;  // UI �Ŵ��� ����

    ScoreEvaluate scoreEvaluate;

    // ������ ������ �迭
    private int[] scores = new int[3];
    private int scoreIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // �ߺ��� GameManager�� ������ �ʵ��� ��
        }

        scoreEvaluate = new ScoreEvaluate();

        // ������ 0���� �ʱ�ȭ
        scores[0] = 0;
        scores[1] = 0;
        scores[2] = 0;
    }

    // �÷��̾��� ������ ������Ʈ�ϴ� �Լ�
    public void UpdatePlayerScore(int _score)
    {
        uiManager.UpdateScore(_score);
    }

    // ���� �����ϰ�, �ʿ��� ��� ���� ������ �Ѿ�� �Լ�
    public void EndTurn()
    {
        // �� ���� �� ó���� ���� (��: ���� �÷��̾�� �Ѿ��)
        // ����: ���� ���� ��
        Debug.Log("Turn Ended. Player's score: " + player.totalScore);
    }

}

