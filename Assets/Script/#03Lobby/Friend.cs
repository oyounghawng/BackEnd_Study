using UnityEngine;
using TMPro;

public class Friend : FriendBase
{
    [SerializeField]
    private TextMeshProUGUI textLevel;

    public override void Setup(BackendFriendSystem FriendSystem, FriendPageBase friendPage, FriendData friendData)
    {
        base.Setup(FriendSystem, friendPage, friendData);

        textLevel.text = friendData.level;
        textTime.text = System.DateTime.Parse(friendData.lastLogin).ToString();
    }

    public void OnClickDeleteFriend()
    {
        //ģ�� ui������Ʈ ����
        friendPageBase.Deactivate(gameObject);
        //ģ�� ��û ����(�鿣��)
        backendFriendSystem.BreakFriend(friendData);
    }
}
