using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Net.NetworkInformation;
public class DailyRankRegister : MonoBehaviour
{
    public void Process(int newScore)
    {
        //UpdateMyRankData(newScore);
        UpdateMyBestRankData(newScore);
        
    }
    private void UpdateMyRankData(int newScore)
    {
        string rowInDate = string.Empty;

        //랭킹 데이터를 업데이트 하기 위해서는 게임 데이터에 사용하는 indate값이 필요
        Backend.GameData.GetMyData(Constants.USER_RANK_DATA_TABLE, new Where(), callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"데이터 소화 중 문제가 발생했습니다. : {callback}");
                return;
            }

            Debug.Log($"데이터 조회에 성공했습니다. : {callback}");

            if (callback.FlattenRows().Count > 0)
            {
                rowInDate = callback.FlattenRows()[0]["inDate"].ToString();
            }
            else
            {
                Debug.LogError("데이터가 존재하지 않습니다.");
                return;
            }

            Param param = new Param()
            {
                {"dailyBestScore", newScore}
            };

            Backend.URank.User.UpdateUserScore(Constants.DALIY_RANK_UUID, Constants.USER_RANK_DATA_TABLE, rowInDate, param
                , callback =>
                {
                    if(callback.IsSuccess())
                    {
                        Debug.Log($"랭킹 등록에 성공했습니다 : {callback}");
                    }
                    else
                    {
                        Debug.LogError($"랭킹등록에 실패했습니다 : {callback}");
                    }
                });

        });
    }
    private void UpdateMyBestRankData(int newScore)
    {
        Backend.URank.User.GetMyRank(Constants.DALIY_RANK_UUID, callback =>
        {
            if(callback.IsSuccess() )
            {
                //json 데이터 파싱 성공
                try
                {
                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    //받아온 데이터의 개수가 0이면 데이터가 없는것
                    if(rankDataJson.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                    }
                    else
                    {
                        //랭킹을 등록 할 때는 컬럼명을 "dailyBestScore로 저장했찌만
                        //불러올 때는 컬럼명이 score로 통일되어 있다.

                        //추가로 등록한 항목은 컬럼명을 그대로 사용
                        int betsScore = int.Parse(rankDataJson[0]["score"].ToString());

                        //현재 점수가 최고 점수보다 높으면
                        if(newScore > betsScore)
                        {
                            UpdateMyRankData(newScore);

                            Debug.Log($"최고 점수 갱신 {betsScore} -> {newScore}");
                        }
                    }
                }
                //json 파싱 실패
                catch(System.Exception e)
                {
                    //try-catch 에러 출력
                    Debug.LogError(e);
                }
            }
            else
            {
                //자신의 랭킹 정보가 존재하지 않을 때는 현재 점수를 새로운 랭킹으로 등록
                if(callback.GetMessage().Contains("userRank"))
                {
                    UpdateMyRankData(newScore);

                    Debug.Log($"새로운 랭킹데이터 생성및 등록 {callback}");
                }
            }
        });
    }
}
