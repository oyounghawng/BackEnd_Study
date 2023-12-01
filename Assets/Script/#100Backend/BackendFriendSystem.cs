using BackEnd;
using System;
using UnityEngine;
public class BackendFriendSystem : MonoBehaviour
{
    [SerializeField]
    private FriendSentRequestPage sentRequestPage;
    private string GetUserInfoBy(string nickname)
    {
        //�ش� �г����� ������ �����ϴ��� ���δ� ����ȭ�� ����
        var bro = Backend.Social.GetUserInfoByNickName(nickname);
        string inDate = string.Empty;

        Debug.Log(bro);
        if (!bro.IsSuccess())
        {
            Debug.LogError($"���� �˻� ���� ������ �߻��߽��ϴ� : {bro}");
            return inDate;
        }
        //json ������ �Ľ� ����
        try
        {
            LitJson.JsonData jsonData = bro.GetFlattenJSON()["row"];

            if (jsonData.Count <= 0)
            {
                Debug.LogWarning("������ inDate ������ �����ϴ�.");
                return inDate;
            }

            inDate = jsonData["inDate"].ToString();

            Debug.Log($"{nickname}�� indate ���� {inDate} �Դϴ�.");
        }
        //json ������ �Ľ̽���
        catch (Exception e)
        {
            //t-c ���� ���
            Debug.LogError(e);
        }

        return inDate;
    }
    public void SendRequestFrined(string nickname)
    {
        //ģ����û �޼ҵ带 ���� ģ����û���ҋ����� indate������ �ʿ�
        string inDate = GetUserInfoBy(nickname);
        //indate ���������� �������� ��û�߼�
        Backend.Friend.RequestFriend(inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"{nickname} ģ�� ��û ���� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            Debug.Log($"ģ�� ��û�� �����߽��ϴ�. {callback}");

            GetSentRequestList();
        });
    }
    public void GetSentRequestList()
    {
        Backend.Friend.GetSentRequestList(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"ģ�� ��û ��� ��� ��ȸ �� ������ �߻��߽��ϴ� : {callback}");
                return;
            }
            //json ������ �Ľ�
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["rows"];

                //�޾ƿ� �������� ������ 0�̸� �����Ͱ� ���°�
                if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("ģ�� ��û ��� ��� �����Ͱ� �����ϴ�.");
                    return;
                }

                //ģ�� ��û ����Ͽ� �ִ� ��� UI��Ȱ��ȭ
                sentRequestPage.DeactivateAll();

                foreach (LitJson.JsonData item in jsonData)
                {
                    FriendData friendData = new FriendData();

                    //friendData.nickname = item.ContainsKey("nickname") == true ? item["nickname"].ToString() : "NONAME";
                    friendData.nickname = item["nickname"].ToString().Equals("True") ? "NONAME" :
                                          item["nickname"].ToString();
                    friendData.inDate = item["inDate"].ToString();
                    friendData.createdAt = item["createdAt"].ToString();

                    //[ģ�� ��û]�� ���� �ð����κ��� ���� �Ⱓ�� �������� �ڵ����� ģ����û ���
                    if(IsExpirationDate(friendData.createdAt))
                    {
                        RevokeSentRequest(friendData.inDate);
                    }
                    //���� frined ������ �������� ģ����û ��� UIȰ��ȭ
                    sentRequestPage.Activate(friendData);
                }
            }

            //json ������ �Ľ̽���
            catch (Exception e)
            {
                //t-c ���� ���
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
                Debug.LogError($"ģ�� ��û ��� ���� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            Debug.Log($"ģ�� ��û ��ҿ� �����߽��ϴ� : {callback}");
        });
    }
    public bool IsExpirationDate(string createdAt)
    {
        //���� �ð� �ҷ�����
        var bro = Backend.Utils.GetServerTime();

        if(!bro.IsSuccess())
        {
            Debug.LogError($"���� �ð� �ҷ����⿡ �����߽��ϴ� . {bro}");
            return false;
        }
        //json ������ �Ľ� ����
        try
        {
            //createdAt �ð����κ��� 3�ϵ� �ð�
            DateTime after3Days = DateTime.Parse(createdAt).AddDays(Constants.EXPIRATION_DAYS);
            //���� �����ð�
            string serverTime = bro.GetFlattenJSON()["utcTime"].ToString();
            //������� ���� �ð� = ����ð� - ���� ���� �ð�
            TimeSpan timeSpan = after3Days - DateTime.Parse(serverTime);

            if(timeSpan.TotalHours < 0)
            {
                return true;
            }    
        }
        //������ �Ľ̽���
        catch (Exception e)
        {
            //t-c ����
            Debug.LogError(e);
        }

        return false;
    }
}
