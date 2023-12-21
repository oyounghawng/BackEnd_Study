using BackEnd;
using UnityEngine;
using UnityEngine.Events;
public class BackendGameData
{
    [System.Serializable]
    public class GameDataLoadEvent : UnityEvent { }
    public GameDataLoadEvent onGameDataLoadEvenet = new GameDataLoadEvent();

    private static BackendGameData instance = null;
    public static BackendGameData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BackendGameData();
            }

            return instance;
        }
    }
    private UserGameData userGameData = new UserGameData();
    public UserGameData UserGameData => userGameData;

    private string gameDataRowIndate = string.Empty;

    /// <summary>
    /// 뒤끝 콘솔 테이블에 새로운 유저정보 추가
    /// </summary>
    public void GameDataInsert()
    {
        //초기값 설정
        userGameData.Reset();

        //테이블에 추가할 데이터로 가공
        Param param = new Param()
        {
            {"level", userGameData.level},
            {"experience", userGameData.experience},
            {"gold", userGameData.gold},
            {"jewel", userGameData.jewel},
            {"heart", userGameData.heart},
        };
        GameDataInsert(Constants.USER_DATA_TABLE, param);
    }
    private void GameRankDataInsert()
    {
        UserGameData.dailyBestScore = 0;

        Param rankParm = new Param()
        {
            {"dailyBestScore", userGameData.dailyBestScore},
        };
        GameDataInsert(Constants.USER_RANK_DATA_TABLE, rankParm);
    }
    private void GameDataInsert(string tableNmae, Param param)
    {
        // 첫번째 매개변수는 뒤끝 콘솔의 "게임정보 관리 탭에 생성한 테이블 이름"
        Backend.GameData.Insert(tableNmae, param, callback =>
        {
            //게임 정보 추가에 성공 했을 때
            if (callback.IsSuccess())
            {
                // 게임 정보의 고유값
                gameDataRowIndate = callback.GetInDate();

                Debug.Log($"게임 정보 데이터 삽입에 성공했습니다 : {callback}");

                onGameDataLoadEvenet?.Invoke();
            }
            //실패
            else
            {
                Debug.Log($"게임 정보 데이터 삽입에 실패했습니다 : {callback}");
            }
        });
    }

    /// <summary>
    /// 뒤끝 콘솔 테이블에서 유저정보 호출
    /// </summary>
    public void GameDataLoad()
    {
        Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
        {
            //성공했을 때
            if (callback.IsSuccess())
            {
                Debug.Log($"게임 정보 데이터 불러오기에 성공했습니다. : {callback}");

                //json 데이터 파싱 성공
                try
                {
                    LitJson.JsonData gameDataJson = callback.FlattenRows();

                    //개수가 0이면 없는거
                    if (gameDataJson.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다.");

                        //유저 정보가 없으면 정보 생성
                        GameDataInsert();
                    }
                    else
                    {
                        //불러온 게임정보의 고유값
                        gameDataRowIndate = gameDataJson[0]["inDate"].ToString();
                        //불러온 게임 정보를 usergamedata변수에 저장
                        UserGameData.level = int.Parse(gameDataJson[0]["level"].ToString());
                        UserGameData.experience = int.Parse(gameDataJson[0]["experience"].ToString());
                        UserGameData.gold = int.Parse(gameDataJson[0]["gold"].ToString());
                        UserGameData.jewel = int.Parse(gameDataJson[0]["jewel"].ToString());
                        UserGameData.heart = int.Parse(gameDataJson[0]["heart"].ToString());

                        onGameDataLoadEvenet?.Invoke();
                    }
                }
                //파싱 실패시 데이터 초기화
                catch (System.Exception e)
                {
                    UserGameData.Reset();
                    // try -cath 에러출력
                    Debug.LogError(e);
                }
            }
            //실패했을 때
            else
            {
                Debug.LogError($"게임정보 데이터 불러오기에 실패했습니다 : {callback}");
            }
        });

        Backend.GameData.GetMyData(Constants.USER_RANK_DATA_TABLE, new Where(), callback =>
        {
            //성공했을 때
            if (callback.IsSuccess())
            {
                Debug.Log($"유저 랭킹 데이터 불러오기에 성공했습니다. : {callback}");

                //json 데이터 파싱 성공
                try
                {
                    LitJson.JsonData gameDataJson = callback.FlattenRows();
                    //개수가 0이면 없는거
                    if (gameDataJson.Count <= 0)
                    {
                        Debug.LogWarning("랭킹 데이터가 존재하지 않습니다.");

                        GameRankDataInsert();
                    }
                }
                //파싱 실패시 데이터 초기화
                catch (System.Exception e)
                {
                    // try -cath 에러출력
                    Debug.LogError(e);
                }
            }
            //실패했을 때
            else
            {
                Debug.LogError($"유저의 랭킹 데이터 불러오기에 실패했습니다 : {callback}");
            }
        });
    }

    //update2와 update 차이는 2같은경우 특정 row 검색가능해서 수정
    /// <summary>
    /// 뒤끝 테이블에 있는 유저 데이터 갱신
    /// </summary>
    public void GameDataUpdate(UnityAction action = null)
    {
        if (userGameData == null)
        {
            Debug.LogError("서버에서 다운 받거나 새로 삽입한 데이터가 존재하지 않습니다." +
                  "Insert 혹은 load를 통해 데이터를 생성해 주세요");
            return;
        }

        Param param = new Param()
        {
            {"level", userGameData.level},
            {"experience", userGameData.experience},
            {"gold", userGameData.gold},
            {"jewel", userGameData.jewel},
            {"heart", userGameData.heart},
        };

        //게임 정보의 고유값(gamedatarowindate)가 없으면 에러메시지 출력
        if (string.IsNullOrEmpty(gameDataRowIndate))
        {
            Debug.LogError("유저의 indate 정보가 없어 게임정보 데이터 수정에 실패했습니다.");
        }
        //게임 정보의 고유값이 있으면 테이블에 저장되어 있는 값중 indat 컬럼의 값과
        //소유하는 유저의 onwerindate가 일치하는 row를 검색하여 수정하는 updatev2호출
        else
        {
            Debug.Log($"{gameDataRowIndate}이 게임 정보 데이터 수정을 요청합니다.");

            Backend.GameData.UpdateV2(Constants.USER_DATA_TABLE, gameDataRowIndate, Backend.UserInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"게임 정보 데이터 수정에 성공했습니다. : {callback}");

                    action?.Invoke();

                    onGameDataLoadEvenet?.Invoke();
                }
                else
                {
                    Debug.LogError($"게임 정보 데이터 수정에 실패했습니다. : {callback}");
                }

            });
        }
    }
}