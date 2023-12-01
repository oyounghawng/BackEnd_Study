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
        //���ӿ��� UIȰ��ȭ
        panelGameOver.SetActive(true);
        //ȹ�� ���� ���
        TextResultScore.text = gameController.Score.ToString();
        //���ӿ��� �ؽ�Ʈ ũŰ ��� �ִϸ��̼�
        effectGameOver.Play(200, 100);
        // 0 => score���� ������ ī�����ϴ� �ִϸ��̼�
        EffectResultScore.Play(0, gameController.Score);
    }

    public void BtnClickGoToLobby()
    {
        Utils.LoadScene(SceneNames.Lobby);
    }
}
