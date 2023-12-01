using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class DailyRankLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject rankDataPrefab;
    [SerializeField]
    private Scrollbar scrollbar;
    [SerializeField]
    private Transform rankDataParent;
    [SerializeField]
    private DailyRankData myRankData;

    private List<DailyRankData> rankDataList;

    private void Awake()
    {
        rankDataList = new List<DailyRankData>();

        //1~20 위 랭킹데티어 출력 
        for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
        {
            GameObject clone = Instantiate(rankDataPrefab, rankDataParent);
            rankDataList.Add(clone.GetComponent<DailyRankData>());
        }
    }

    private void OnEnable()
    {
        //1위 랭킹이 보이도록 값설정
        scrollbar.value = 1;
        // 1~20 위 랭킹 정보 불러오기
        GetRankList();
        //내 랭킹 정보 불러오기
        GetMyRank();
    }
    private void GetRankList()
    {
        //1~20위 랭킹정보 불러오기
        Backend.URank.User.GetRankList(Constants.DALIY_RANK_UUID, Constants.MAX_RANK_LIST, callback =>
        {
            if (callback.IsSuccess())
            {
                //json 데이터 파싱성공
                try
                {
                    Debug.Log($"랭킹 조회에 성공했습니다. : {callback}");

                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    if (rankDataJson.Count <= 0)
                    {
                        //1~20 위 빈 데이터로 설정
                        for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
                        {
                            SetRankData(rankDataList[i], i + 1, "-", 0);

                        }
                        Debug.LogWarning("데이터가 존재하지 않습니다");
                    }
                    else
                    {
                        int rankerCount = rankDataJson.Count;

                        for (int i = 0; i < rankerCount; ++i)
                        {
                            rankDataList[i].Rank = int.Parse(rankDataJson[i]["rank"].ToString());
                            rankDataList[i].Score = int.Parse(rankDataJson[i]["score"].ToString());

                            //닉네임은 별도로 설정하지 않은 유저도 존재 할 수 있기 때문에
                            //닉넴인이 존재하지 않는 유저는 닉네임 대신 gamerID출력
                            rankDataList[i].Nickname = rankDataJson[i].ContainsKey("Nickname") == true ?
                                                        rankDataJson[i]["nickname"]?.ToString() : UserInfo.Data.gamerID;
                        }
                        //만약 rankerCount 가 20위까지 존재하지 않으면 나머지는 빈 데이터를 설정
                        for (int i = rankerCount; i < Constants.MAX_RANK_LIST; ++i)
                        {
                            SetRankData(rankDataList[i], i + 1, "-", 0);
                        }
                    }
                }
                //json 파싱에러
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                //1~20위까지 빈데이터로 설정
                for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
                {
                    SetRankData(rankDataList[i], i + 1, "-", 0);
                }

                Debug.LogError($"랭킹 조회 중 오류가 발생했습니다.");
            }
        });
    }
    private void GetMyRank()
    {
        //내 랭킹정보 불러오기
        Backend.URank.User.GetMyRank(Constants.DALIY_RANK_UUID, callback =>
        {
            //닉네임이 없으면 GamerID 있으면 닉네임 사용
            string nickname = UserInfo.Data.nickname == null ? UserInfo.Data.gamerID : UserInfo.Data.nickname;

            if (callback.IsSuccess())
            {
                //json 데이터 파싱성공
                try
                {
                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    //받아온 데이터의 개수가 0이면 데이터가 없는 것
                    if (rankDataJson.Count <= 0)
                    {
                        // ["순위에 없음","닉네임",0] 형식으로 출력
                        SetRankData(myRankData, 1000000000, nickname, 0);
                        Debug.LogWarning("데이터가 존재하지 않습니다");
                    }

                    else
                    {
                        myRankData.Rank = int.Parse(rankDataJson[0]["rank"].ToString());
                        myRankData.Score = int.Parse(rankDataJson[0]["score"].ToString());

                        //닉네임은 별도로 설정하지 않은 유저도 존재 할 수 있기 때문에
                        //닉넴인이 존재하지 않는 유저는 닉네임 대신 gamerID출력
                        myRankData.Nickname = rankDataJson[0].ContainsKey("Nickname") == true ?
                                                    rankDataJson[0]["nickname"]?.ToString() : UserInfo.Data.gamerID;
                    }
                }
                //json 파싱에러
                catch (System.Exception e)
                {
                    // ["순위에 없음","닉네임",0] 형식으로 출력
                    SetRankData(myRankData, 1000000000, nickname, 0);
                    //try - catch 에러
                    Debug.LogError(e);
                }
            }
            else
            {
                if(callback.GetMessage().Contains("userRank"))
                {
                    // ["순위에 없음","닉네임",0] 형식으로 출력
                    SetRankData(myRankData, 1000000000, nickname, 0);
                }
            }

        });
    }
    private void SetRankData(DailyRankData rankData, int rank, string nickname, int score)
    {
        rankData.Rank = rank;
        rankData.Nickname = nickname;
        rankData.Score = score;
    }
}
