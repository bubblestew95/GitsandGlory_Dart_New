using UnityEngine;

public class DartGameManager : MonoBehaviour
{
    public static DartGameManager Instance;
    public DartPlayer player;  // 플레이어 참조
    public UIManager uiManager;  // UI 매니저 참조

    ScoreEvaluate scoreEvaluate;

    // 점수를 저장할 배열
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
            Destroy(gameObject);  // 중복된 GameManager가 생기지 않도록 함
        }

        // 점수를 0으로 초기화
        scores[0] = 0;
        scores[1] = 0;
        scores[2] = 0;
    }
    private void Update()
    {
        TestCoordinate();
    }

    // 플레이어의 점수를 업데이트하는 함수
    public void UpdatePlayerScore(int _score)
    {
        uiManager.UpdateScore(_score);
    }

    // 턴을 종료하고, 필요한 경우 다음 턴으로 넘어가는 함수
    public void EndTurn()
    {
        // 턴 종료 후 처리할 로직 (예: 다음 플레이어로 넘어가기)
        // 예시: 게임 종료 시
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

            // 점수를 배열에 저장
            if (scoreIndex < 3)
            {
                scores[scoreIndex] = score;
                scoreIndex++;
            }

            // 점수를 UIManager를 통해 UI에 반영
            uiManager.UpdateScoreUI(scores);

        }
    }

}

