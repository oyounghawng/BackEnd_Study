using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendSentRequest : FriendBase
{
    public override void Setup(BackendFriendSystem FriendSystem, FriendPageBase friendPage, FriendData friendData)
    {
        base.Setup(FriendSystem, friendPage, friendData);
        base.SetExpirationDate();
    }

    public void OnClickCancelRequest()
    {
        //ģ�� UI������Ʈ ��Ȱ��ȭ
        friendPageBase.Deactivate(gameObject);
        //ģ�� ��û ���
        backendFriendSystem.RevokeSentRequest(friendData.inDate);
    }
}
