using UnityEngine;

public class DartGameManager : MonoBehaviour
{
    public static DartGameManager Instance;
    public DartPlayer player;  // �÷��̾� ����
    public UIManager uiManager;  // UI �Ŵ��� ����

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

