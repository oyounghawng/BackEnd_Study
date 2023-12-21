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
        //친구 ui오브젝트 삭제
        friendPageBase.Deactivate(gameObject);
        //친구 요청 수락(백엔드)
        backendFriendSystem.BreakFriend(friendData);
    }
}
