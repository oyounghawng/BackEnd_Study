using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PopupPostBox : MonoBehaviour
{
    [SerializeField]
    private BackendPostSystem backendPostSystem;// 우편 수령을 누르기위핸 포스트시스템
    [SerializeField]
    private GameObject postPrefab; //우편 ui프리팹
    [SerializeField]
    private Transform parentContent; // 우편 ui가 배치되는 스크롤뷰
    [SerializeField]
    private GameObject textSystem; // 우편함이 비었을때 텍스트 오브젝트

    private List<GameObject> postList;

    private void Awake()
    {
        postList = new List<GameObject>();
    }

    private void OnDisable()
    {
        DestroyPostAll();
    }
    public void SpawnPostAll(List<PostData> postDataList)
    {
        for(int i = 0; i < postDataList.Count; ++i)
        {
            GameObject clone = Instantiate(postPrefab, parentContent);
            clone.GetComponent<Post>().Setup(backendPostSystem, this, postDataList[i]);
            postList.Add(clone);
        }

        textSystem.SetActive(false);
    }
    public void DestroyPostAll()
    {
        foreach( GameObject post in postList )
        {
            if (post != null) Destroy(post);
        }

        postList.Clear();

        textSystem.SetActive(true);
    }
    public void DestroyPost(GameObject post)
    {
        Destroy(post);
        postList.Remove(post);

        if(postList.Count == 0)
        {
            textSystem.SetActive(false);
        }
    }
}
