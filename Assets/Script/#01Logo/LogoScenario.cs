using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScanrio : MonoBehaviour
{
    [SerializeField]
    private Progress progress;
    [SerializeField]
    private SceneNames nextScene;
    private void Awake()
    {
        SystemSetup();
    }
    private void SystemSetup()
    {
        // Ȱ��ȭ ���� �������¿����� ���� ����

        Application.runInBackground = true;

        //�ػ� ����(9:18.5 , 1440��2960)
        int width = Screen.width;
        int height = (int)(Screen.width*18.5f/9);
        Screen.SetResolution(width, height, true);

        //ȭ���� �������ʵ���
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        progress.Play(OnAfterProgress);
    }

    private void OnAfterProgress()
    {
        Utils.LoadScene(nextScene);
    }
}
