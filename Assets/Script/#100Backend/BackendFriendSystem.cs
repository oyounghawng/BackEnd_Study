using BackEnd;
using System;
using System.Collections.Generic;
using UnityEngine;
public class BackendFriendSystem : MonoBehaviour
{
    [SerializeField]
    private FriendSentRequestPage sentRequestPage;
    [SerializeField]
    private FriendReceivedRequestPage receivedRequestPage;
    [SerializeField]
    private FriendPage friendPage;
    private string GetUserInfoBy(string nickname)
    {
        //해당 닉네임의 유저가 존재하는지 여부는 동기화로 진행
        var bro = Backend.Social.GetUserInfoByNickName(nickname);
        string inDate = string.Empty;

        Debug.Log(bro);
        if (!bro.IsSuccess())
        {
            Debug.LogError($"유저 검색 도중 에러가 발생했습니다 : {bro}");
            return inDate;
        }
        //json 데이터 파싱 성공
        try
        {
            LitJson.JsonData jsonData = bro.GetFlattenJSON()["row"];

            if (jsonData.Count <= 0)
            {
                Debug.LogWarning("유저의 inDate 정보가 없습니다.");
                return inDate;
            }

            inDate = jsonData["inDate"].ToString();

            Debug.Log($"{nickname}의 indate 값은 {inDate} 입니다.");
        }
        //json 데이터 파싱실패
        catch (Exception e)
        {
            //t-c 오류 출력
            Debug.LogError(e);
        }

        return inDate;
    }
    public void SendRequestFriend(string nickname)
    {
        //친구요청 메소드를 통해 친구요청을할떄에는 indate정보가 필요
        string inDate = GetUserInfoBy(nickname);
        //indate 정보를가진 유저에게 요청발송
        Backend.Friend.RequestFriend(inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"{nickname} 친구 요청 도중 에러가 발생했습니다. : {callback}");
                return;
            }

            Debug.Log($"친구 요청에 성공했습니다. {callback}");

            GetSentRequestList();
        });
    }
    public void GetSentRequestList()
    {
        Backend.Friend.GetSentRequestList(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"친구 요청 대기 목록 조회 중 에러가 발생했습니다 : {callback}");
                return;
            }
            //json 데이터 파싱
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["rows"];

                //받아온 데이터의 개수가 0이면 데이터가 없는것
                if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("친구 요청 대기 목록 데이터가 없습니다.");
                    return;
                }

                //친구 요청 대기목록에 있는 모든 UI비활성화
                sentRequestPage.DeactivateAll();

                foreach (LitJson.JsonData item in jsonData)
                {
                    FriendData friendData = new FriendData();

                    //friendData.nickname = item.ContainsKey("nickname") == true ? item["nickname"].ToString() : "NONAME";
                    friendData.nickname = item["nickname"].ToString().Equals("True") ? "NONAME" :
                                          item["nickname"].ToString();
                    friendData.inDate = item["inDate"].ToString();
                    friendData.createdAt = item["createdAt"].ToString();

                    //[친구 요청]을 보낸 시간으로부터 일정 기간이 지났다면 자동으로 친구요청 취소
                    if (IsExpirationDate(friendData.createdAt))
                    {
                        RevokeSentRequest(friendData.inDate);
                    }
                    //현재 frined 정보를 바탕으로 친구요청 대기 UI활성화
                    sentRequestPage.Activate(friendData);
                }
            }

            //json 데이터 파싱실패
            catch (Exception e)
            {
                //t-c 오류 출력
                Debug.LogError(e);
            }
        });
    }
    public void RevokeSentRequest(string inDate)
    {
        Backend.Friend.RevokeSentRequest(inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"친구 요청 취소 도중 에러가 발생했습니다. : {callback}");
                return;
            }

            Debug.Log($"친구 요청 취소에 성공했습니다 : {callback}");
        });
    }
    public void GetReceiveRequestList()
    {
        Backend.Friend.GetReceivedRequestList(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"친구 수락 대기 목록 조회 도중 에러가 발생했습니다 : {callback}");
            }

            //json 데이터 개수 0
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["rows"];

                //받아온 데이터의 개수가 0이면 데이터가 없는것
                if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("친구 수락 대기 목록 데이터가 없습니다.");
                    return;
                }

                //친구 수락 대기 목록에 있는 모든 UI비활성화
                receivedRequestPage.DeactivateAll();

                foreach (LitJson.JsonData item in jsonData)
                {
                    FriendData friendData = new FriendData();

                    //friendData.nickname = item.ContainsKey("nickname") == true ? item["nickname"].ToString() : "NONAME";
                    friendData.nickname = item["nickname"].ToString().Equals("True") ? "NONAME" :
                                          item["nickname"].ToString();
                    friendData.inDate = item["inDate"].ToString();
                    friendData.createdAt = item["createdAt"].ToString();

                    //[친구 요청]을 보낸 시간으로부터 일정 기간이 지났따면 자동으로 친구요청 취소
                    if (IsExpirationDate(friendData.createdAt))
                    {
                        RejectFriend(friendData);
                        continue;
                    }

                    //현재 freindData 정보를 바탕으로 수락대기 활성화
                    receivedRequestPage.Activate(friendData);
                }
            }

            //json 데이터 파싱실패
            catch (Exception e)
            {
                //t-c 오류 출력
                Debug.LogError(e);
            }
        });
    }
    public void AcceptFriend(FriendData friendData)
    {
        Backend.Friend.AcceptFriend(friendData.inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"친구 수락 중 에러가 발생했습니다 : {callback}");
                return;
            }

            Debug.Log($"{friendData.nickname}이(가) 친구가 되었습니다 : {callback}");
        });
    }
    public void RejectFriend(FriendData friendData)
    {
        Backend.Friend.RejectFriend(friendData.inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"친구 거절 중 에러가 발생했습니다 : {callback}");
                return;
            }

            Debug.Log($"{friendData.nickname}의 친구 요청을 거절했습니다. : {callback}");
        });
    }
    public void GetFriendList()
    {
        Backend.Friend.GetFriendList(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"친구 목록 조회 중 에러가 발생했습니다 : {callback}");
                return;
            }

            //json 데이터 파싱성공
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["rows"];

                //받아온 데이터의 개수가 0이면 데이터가 없는것
                if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("친구 목록 데이터가 없습니다.");
                    return;
                }

                friendPage.DeactivateAll();

                //친구 수락 대기 목록에 있는 모든 UI비활성화
                List<TransactionValue> transactionList = new List<TransactionValue>();
                List<FriendData> friendDataList = new List<FriendData>();

                foreach (LitJson.JsonData item in jsonData)
                {
                    FriendData friendData = new FriendData();

                    //friendData.nickname = item.ContainsKey("nickname") == true ? item["nickname"].ToString() : "NONAME";
                    friendData.nickname = item["nickname"].ToString().Equals("True") ? "NONAME" :
                                          item["nickname"].ToString();
                    friendData.inDate = item["inDate"].ToString();
                    friendData.createdAt = item["createdAt"].ToString();
                    friendData.lastLogin = item["lastLogin"].ToString();

                    friendDataList.Add(friendData);

                    //데이타 인데이트를 가지는 친구의 유저 정보 가져오기
                    Where where = new Where();
                    where.Equal("owner_inDate", friendData.inDate);
                    transactionList.Add(TransactionValue.SetGet(Constants.USER_DATA_TABLE, where));
                }

                Backend.GameData.TransactionReadV2(transactionList, callback =>
                {
                    if (!callback.IsSuccess())
                    {
                        Debug.LogError($"Transaction Error : {callback}");
                        return;
                    }

                    LitJson.JsonData userData = callback.GetFlattenJSON()["Responses"];

                    if (userData.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                        return;
                    }

                    for (int i = 0; i < userData.Count; ++i)
                    {
                        //친구 레벨 정보 설정
                        friendDataList[i].level = $"Lv. {userData[i]["level"]}";
                        // 현재 정보를 바탕으로 친구 ui활성화
                        friendPage.Activate(friendDataList[i]);
                    }
                });
            }
            //json 데이터 파싱실패
            catch (Exception e)
            {
                //t-c 오류 출력
                Debug.LogError(e);
            }
        });
    }
    public void BreakFriend(FriendData friendData)
    {
        Backend.Friend.BreakFriend(friendData.inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"친구 삭제 중 에러가 발생했습니다 : {callback}");
                return;
            }

            Debug.Log($"{friendData.nickname}의 친구가 해제되었습니다. : {callback}");
        });
    }
    public bool IsExpirationDate(string createdAt)
    {
        //서버 시간 불러오기
        var bro = Backend.Utils.GetServerTime();

        if (!bro.IsSuccess())
        {
            Debug.LogError($"서버 시간 불러오기에 실패했습니다 . {bro}");
            return false;
        }
        //json 데이터 파싱 성공
        try
        {
            //createdAt 시간으로부터 3일뒤 시간
            DateTime after3Days = DateTime.Parse(createdAt).AddDays(Constants.EXPIRATION_DAYS);
            //현재 서버시간
            string serverTime = bro.GetFlattenJSON()["utcTime"].ToString();
            //만료까지 남은 시간 = 만료시간 - 현재 서버 시간
            TimeSpan timeSpan = after3Days - DateTime.Parse(serverTime);

            if (timeSpan.TotalHours < 0)
            {
                return true;
            }
        }
        //데이터 파싱실패
        catch (Exception e)
        {
            //t-c 에러
            Debug.LogError(e);
        }

        return false;
    }
}
