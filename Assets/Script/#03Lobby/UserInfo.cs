using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
public class UserInfo : MonoBehaviour
{
    [System.Serializable]
    public class UserInfoEvent : UnityEngine.Events.UnityEvent
    {

    }
    public UserInfoEvent onUserInfoEvent = new UserInfoEvent();

    private static UserInfoData data = new UserInfoData();
    public static UserInfoData Data => data;

    public void GetUserInfoFromBackend()
    {
        //현재 로그인한 사용자 정보 불러오기
        Backend.BMember.GetUserInfo(callback =>
        {
            //불러오기 성공
            if(callback.IsSuccess())
            {
                //JSON 데이터 파싱성공
                try
                {
                    JsonData json = callback.GetReturnValuetoJSON()["row"];

                    data.gamerID = json["gamerId"].ToString();
                    data.countryCode = json["countryCode"]?.ToString();
                    data.nickname = json["nickname"]?.ToString();
                    data.inDate = json["inDate"].ToString();
                    data.emailForFindPassword = json["emailForFindPassword"]?.ToString();
                    data.subscriptionType = json["subscriptionType"].ToString();
                    data.federationId = json["federationId"]?.ToString();
                }        
                //데이터 파싱 실패
                catch(System.Exception e)
                {
                    //기본상태 설정
                    data.Reset();
                    //try-catch 에러 출력
                    Debug.LogError(e);
                }
            }
            //실패
            else
            {
                //유저 정보를 기본상태로 설정
                //TIP 일반적으로 오프라인 상태를 대비해 기본적인 정보를 저장해두고 오프라인일떄 불러와 사용
                data.Reset();
                Debug.LogError(callback.GetMessage());
            }
            //성공시 ui출력 이벤트 메소드 호출
            onUserInfoEvent?.Invoke();
        });
    }
}
public class UserInfoData
{
    public string gamerID;
    public string countryCode;
    public string nickname;
    public string inDate;
    public string emailForFindPassword;
    public string subscriptionType;
    public string federationId;

    public void Reset()
    {
        gamerID = "Offline";
        countryCode = "UnKnown";
        nickname = "NoName";
        inDate = string.Empty;
        emailForFindPassword = string.Empty;
        subscriptionType = string.Empty;
        federationId = string.Empty;
    }
}
