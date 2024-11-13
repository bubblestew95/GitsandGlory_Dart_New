using UnityEngine;

public class DartGameManager : MonoBehaviour
{
    public static DartGameManager Instance;
    public DartPlayer player;  // 플레이어 참조
    public UIManager uiManager;  // UI 매니저 참조

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // 중복된 GameManager가 생기지 않도록 함
        }
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
}

