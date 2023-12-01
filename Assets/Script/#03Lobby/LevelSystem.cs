using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    private readonly int increaseExperience = 25; //플래이당 경험치

    public void Process()
    {
        int currentLevel = BackendGameData.Instance.UserGameData.level;

        //경험치 증가 및 레벨업 여부 검사
        //현재 레벨 시스템에 대한 설정이 없기에 최대값 100
        //플래이 할때마다 25씩 증가
        BackendGameData.Instance.UserGameData.experience += increaseExperience;
        if (BackendGameData.Instance.UserGameData.experience >= BackendChartData.levelChart[currentLevel - 1].maxExperience
            && BackendChartData.levelChart.Count > currentLevel)
        {
            //레벌업 보상 지급
            BackendGameData.Instance.UserGameData.gold += BackendChartData.levelChart[currentLevel - 1].rewardGold;
            //경험치를 0 으로 초기화
            BackendGameData.Instance.UserGameData.experience = 0;
            //레벨 1증가
            BackendGameData.Instance.UserGameData.level++;
        }

        //게임정보 업데이트
        BackendGameData.Instance.GameDataUpdate();

        Debug.Log($"현재 레벨 : {BackendGameData.Instance.UserGameData.level}," +
                  $"경험치 : {BackendGameData.Instance.UserGameData.experience}," +
                  $"다음 레벨업까지 경험치 : {BackendChartData.levelChart[currentLevel-1].maxExperience},");
    }
}
