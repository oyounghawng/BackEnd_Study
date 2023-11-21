using UnityEngine;
using BackEnd; // 뒤끝 SDK
public class BackendManager : MonoBehaviour
{
    private void Awake()
    {
        //Update메소드의 backendpoll 호출을 위해 오브젝트를 파괴하지않는다.
        DontDestroyOnLoad(gameObject);

        //서버초기화
        BackendSetup();
    }

    private void Update()
    {
        //서버의 비동기 메소드 호출
        if(Backend.IsInitialized)
        {
            Backend.AsyncPoll();
        }
    }

    private void BackendSetup()
    {
        //초기화
        var bro = Backend.Initialize(true);

        // 초기화 응닶
        if(bro.IsSuccess())
        {
            Debug.Log($"초기화 성공 : {bro}");
            
        }
        else
        {
            Debug.Log($"초기화 실패 : {bro}");
        }
    }
}
