using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendPageBase : MonoBehaviour
{
    [Header("Common")]
    [SerializeField]
    protected BackendFriendSystem backendFriendSystem;

    [Header("Friend Page Base")]
    [SerializeField]
    private GameObject frinedPrefab;
    [SerializeField]
    private Transform parentContent;
    [SerializeField]
    private GameObject textSystem;

    private MemoryPool memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool(frinedPrefab, parentContent);
    }
    public void Activate(FriendData friend)
    {
        if (textSystem.activeSelf) textSystem.SetActive(false);

        GameObject item = memoryPool.ActivatePoolItem();
        item.GetComponent<FriendBase>().Setup(backendFriendSystem, this, friend);
    }
    public void ActivateAll(List<FriendData> friendList)
    {
        for(int i = 0; i < friendList.Count; ++i)
        {
            Activate(friendList[i]);
        }
    }
    public void DeactivateAll()
    {
        textSystem.SetActive(true);

        memoryPool.DeactivateAllPoolItems();
    }
    public void Deactivate(GameObject frined)
    {
        memoryPool.DeactivatePoolItem(frined);

        if(memoryPool.ActiveCount == 0)
        {
            textSystem.SetActive(true);
        }
    }
}
