using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendReceivedRequestPage : FriendPageBase
{
    private void OnEnable()
    {
        //[ģ���������]��� �ҷ�����
        backendFriendSystem.GetReceiveRequestList();
    }
    private void OnDisable()
    {
        DeactivateAll();
    }
}
