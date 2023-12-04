public class FriendReceivedRequest : FriendBase
{
    public override void Setup(BackendFriendSystem FriendSystem, FriendPageBase friendPage, FriendData friendData)
    {
        base.Setup(FriendSystem, friendPage, friendData);
        base.SetExpirationDate();
    }
    
    public void OnClickAcceptRequest()
    {
        //친구 ui오브젝트 삭제
        friendPageBase.Deactivate(gameObject);
        //친구 요청 수락(백엔드)
        backendFriendSystem.AcceptFriend(friendData);
    }
    public void OnClickRejectRequest()
    {
        //친구 ui오브젝트 삭제
        friendPageBase.Deactivate(gameObject);
        //친구 요청 수락(백엔드)
        backendFriendSystem.RejectFriend(friendData);
    }
}
