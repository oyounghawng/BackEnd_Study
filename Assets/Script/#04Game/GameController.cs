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
        //중복 처리 되지 않도록 bool 변수로 제어
        if (IsGameOver == true) return;

        IsGameOver = false;

        //게임오버 되었을때 호출할 매소드 실행
        onGameOver.Invoke();

        //현재 점수 정보를 바탕으로 랭크 갱신
        dailyrank.Process(score);
    }
}
