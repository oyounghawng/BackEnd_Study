using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using Unity.VisualScripting;
public class BackendChartData : MonoBehaviour
{
    //레벨별 레벨업 필요 경험치와 보상정보
    public static List<LevelChartData> levelChart;

    static BackendChartData()
    {
        levelChart = new List<LevelChartData>();
    }

    /// <summary>
    /// 차트가 여러개일수도 있음 모든 차트 불러오기
    /// </summary>
    public static void LoadAllChart()
    {
        LoadLevelChart();
    }

    public static void LoadLevelChart()
    {
        Backend.Chart.GetChartContents(Constants.LEVEL_CHART, callback =>
        {
            if(callback.IsSuccess())
            {
                //json 데이터 파싱 성공
                try
                {
                    LitJson.JsonData jsonData = callback.FlattenRows();

                    //받아온 데이터의 개수가 0이면 데이터가 없는것
                    if(jsonData.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                    }
                    else
                    {
                        for(int i = 0; i < jsonData.Count; ++i)
                        {
                            LevelChartData newChart = new LevelChartData();
                            newChart.level = int.Parse(jsonData[i]["level"].ToString());
                            newChart.maxExperience = int.Parse(jsonData[i]["maxExperience"].ToString());
                            newChart.rewardGold = int.Parse(jsonData[i]["rewardGold"].ToString());

                            levelChart.Add(newChart);

                            Debug.Log($"Level : {newChart.level}, max Exp : {newChart.maxExperience}," +
                                      $"Reward Gold : {newChart.rewardGold}");
                        }
                    }
                }
                catch (System.Exception e)
                {
                    //try - catch
                    Debug.LogError(e);
                }
            }
            else
            {
                Debug.LogError($"{Constants.LEVEL_CHART}의 차트 불러오기에 에러 발생 : {callback}");
            }
        });
    }
}

[System.Serializable]
public class LevelChartData
{
    public int level;
    public int maxExperience;
    public int rewardGold;
}
