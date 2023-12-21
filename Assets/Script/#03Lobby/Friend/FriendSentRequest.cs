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
        //친구 UI오브젝트 비활성화
        friendPageBase.Deactivate(gameObject);
        //친구 요청 취소
        backendFriendSystem.RevokeSentRequest(friendData.inDate);
    }
}
