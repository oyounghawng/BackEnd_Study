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
        //���� �α����� ����� ���� �ҷ�����
        Backend.BMember.GetUserInfo(callback =>
        {
            //�ҷ����� ����
            if(callback.IsSuccess())
            {
                //JSON ������ �Ľ̼���
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
                //������ �Ľ� ����
                catch(System.Exception e)
                {
                    //�⺻���� ����
                    data.Reset();
                    //try-catch ���� ���
                    Debug.LogError(e);
                }
            }
            //����
            else
            {
                //���� ������ �⺻���·� ����
                //TIP �Ϲ������� �������� ���¸� ����� �⺻���� ������ �����صΰ� ���������ϋ� �ҷ��� ���
                data.Reset();
                Debug.LogError(callback.GetMessage());
            }
            //������ ui��� �̺�Ʈ �޼ҵ� ȣ��
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
