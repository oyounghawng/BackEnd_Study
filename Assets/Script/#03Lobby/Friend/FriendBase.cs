using BackEnd;
using System;
using TMPro;
using UnityEngine;
public class FriendBase : MonoBehaviour
{
    [Header("Friend Base")]
    [SerializeField]
    private TextMeshProUGUI textNickname; //�г���
    [SerializeField]
    protected TextMeshProUGUI textTime; //����ð�,���ӽð� ���� �ð� ����

    protected BackendFriendSystem backendFriendSystem;
    protected FriendPageBase friendPageBase;
    protected FriendData friendData;

    public virtual void Setup(BackendFriendSystem FriendSystem, FriendPageBase friendPage, FriendData friendData)
    {
        backendFriendSystem = FriendSystem;
        friendPageBase = friendPage;
        this.friendData = friendData;

        textNickname.text = friendData.nickname;
    }
    public void SetExpirationDate()
    {
        //GetServerTime() - �����ð� �ҷ�����
        Backend.Utils.GetServerTime(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"���� �ð� �ҷ����⿡ �����߽��ϴ�. : {callback}");
                return;
            }

            //json ������ �Ľ� ����
            try
            {
                //createdAt �ð����κ��� 3�ϵ� �ð�
                DateTime after3Days = DateTime.Parse(friendData.createdAt).AddDays(Constants.EXPIRATION_DAYS);
                //���� �����ð�
                string serverTime = callback.GetFlattenJSON()["utcTime"].ToString();
                //������� ���� �ð� = ����ð� - ���� ���� �ð�
                TimeSpan timeSpan = after3Days - DateTime.Parse(serverTime);

                textTime.text = $"{timeSpan.TotalHours:F0}�ð� ����";
            }
            //������ �Ľ̽���
            catch (Exception e)
            {
                //t-c ����
                Debug.LogError(e);
            }
        });
    }
}
