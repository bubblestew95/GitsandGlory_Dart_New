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
        scoreEvaluate = new ScoreEvaluate();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // �ߺ��� GameManager�� ������ �ʵ��� ��
        }

        // ������ 0���� �ʱ�ȭ
        scores[0] = 0;
        scores[1] = 0;
        scores[2] = 0;
    }
    private void Update()
    {
        TestCoordinate();
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

    public void TestCoordinate()
    {
        Vector2 coordinate = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            coordinate = new Vector2(
                Random.Range(-10.00f, 10.00f),
                Random.Range(-10.00f, 10.00f)
                );
            Debug.Log("coordinate : " + coordinate);

            int score = scoreEvaluate.EvaluateScore(coordinate);

            // ������ �迭�� ����
            if (scoreIndex < 3)
            {
                scores[scoreIndex] = score;
                scoreIndex++;
            }

            // ������ UIManager�� ���� UI�� �ݿ�
            uiManager.UpdateScoreUI(scores);

        }
    }

}

