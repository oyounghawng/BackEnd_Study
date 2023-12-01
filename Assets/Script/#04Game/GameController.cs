using UnityEngine;
using UnityEngine.Events;
public class GameController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onGameOver;
    [SerializeField]
    private DailyRankRegister dailyrank;
    private int score = 0;
    public bool IsGameOver { set; get; } = false;
    public int Score
    {
        set => score = Mathf.Max(0, value);
        get => score;
    }
    public void GameOver()
    {
        //�ߺ� ó�� ���� �ʵ��� bool ������ ����
        if (IsGameOver == true) return;

        IsGameOver = false;

        //���ӿ��� �Ǿ����� ȣ���� �żҵ� ����
        onGameOver.Invoke();

        //���� ���� ������ �������� ��ũ ����
        dailyrank.Process(score);
    }
}
