using UnityEngine;
using BackEnd; // �ڳ� SDK
public class BackendManager : MonoBehaviour
{
    private void Awake()
    {
        //Update�޼ҵ��� backendpoll ȣ���� ���� ������Ʈ�� �ı������ʴ´�.
        DontDestroyOnLoad(gameObject);

        //�����ʱ�ȭ
        BackendSetup();
    }

    private void Update()
    {
        //������ �񵿱� �޼ҵ� ȣ��
        if(Backend.IsInitialized)
        {
            Backend.AsyncPoll();
        }
    }

    private void BackendSetup()
    {
        //�ʱ�ȭ
        var bro = Backend.Initialize(true);

        // �ʱ�ȭ ����
        if(bro.IsSuccess())
        {
            Debug.Log($"�ʱ�ȭ ���� : {bro}");
            
        }
        else
        {
            Debug.Log($"�ʱ�ȭ ���� : {bro}");
        }
    }
}
