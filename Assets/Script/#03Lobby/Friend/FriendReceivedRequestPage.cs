using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendReceivedRequestPage : FriendPageBase
{
    private void OnEnable()
    {
        //[친구수락대기]목록 불러오기
        backendFriendSystem.GetReceiveRequestList();
    }
    private void OnDisable()
    {
        DeactivateAll();
    }
}
