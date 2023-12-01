using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    private readonly int increaseExperience = 25; //�÷��̴� ����ġ

    public void Process()
    {
        int currentLevel = BackendGameData.Instance.UserGameData.level;

        //����ġ ���� �� ������ ���� �˻�
        //���� ���� �ý��ۿ� ���� ������ ���⿡ �ִ밪 100
        //�÷��� �Ҷ����� 25�� ����
        BackendGameData.Instance.UserGameData.experience += increaseExperience;
        if (BackendGameData.Instance.UserGameData.experience >= BackendChartData.levelChart[currentLevel - 1].maxExperience
            && BackendChartData.levelChart.Count > currentLevel)
        {
            //������ ���� ����
            BackendGameData.Instance.UserGameData.gold += BackendChartData.levelChart[currentLevel - 1].rewardGold;
            //����ġ�� 0 ���� �ʱ�ȭ
            BackendGameData.Instance.UserGameData.experience = 0;
            //���� 1����
            BackendGameData.Instance.UserGameData.level++;
        }

        //�������� ������Ʈ
        BackendGameData.Instance.GameDataUpdate();

        Debug.Log($"���� ���� : {BackendGameData.Instance.UserGameData.level}," +
                  $"����ġ : {BackendGameData.Instance.UserGameData.experience}," +
                  $"���� ���������� ����ġ : {BackendChartData.levelChart[currentLevel-1].maxExperience},");
    }
}
