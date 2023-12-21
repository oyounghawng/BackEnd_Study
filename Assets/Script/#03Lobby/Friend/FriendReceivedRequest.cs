public class FriendReceivedRequest : FriendBase
{
    public override void Setup(BackendFriendSystem FriendSystem, FriendPageBase friendPage, FriendData friendData)
    {
        base.Setup(FriendSystem, friendPage, friendData);
        base.SetExpirationDate();
    }
    
    public void OnClickAcceptRequest()
    {
        //ģ�� ui������Ʈ ����
        friendPageBase.Deactivate(gameObject);
        //ģ�� ��û ����(�鿣��)
        backendFriendSystem.AcceptFriend(friendData);
    }
    public void OnClickRejectRequest()
    {
        //ģ�� ui������Ʈ ����
        friendPageBase.Deactivate(gameObject);
        //ģ�� ��û ����(�鿣��)
        backendFriendSystem.RejectFriend(friendData);
    }
}
