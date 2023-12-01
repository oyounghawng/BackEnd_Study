using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    [Header("InGame")]
    [SerializeField]
    private TextMeshProUGUI textScore;

    [Header("Game Over")]
    [SerializeField]
    private GameObject panelGameOver;
    [SerializeField]
    private TextMeshProUGUI TextResultScore;

    [Header("Game Over UI Animation")]
    [SerializeField]
    private ScaleEffect effectGameOver;
    [SerializeField]
    private CountingEffect EffectResultScore;
    private void Update()
    {
        textScore.text = $"SCORE {gameController.Score}";
    }

    public void OnGameOver()
    {
        //게임오버 UI활성화
        panelGameOver.SetActive(true);
        //획득 점수 출력
        TextResultScore.text = gameController.Score.ToString();
        //게임오버 텍스트 크키 축소 애니메이션
        effectGameOver.Play(200, 100);
        // 0 => score까지 점수를 카운팅하는 애니메이션
        EffectResultScore.Play(0, gameController.Score);
    }

    public void BtnClickGoToLobby()
    {
        Utils.LoadScene(SceneNames.Lobby);
    }
}
