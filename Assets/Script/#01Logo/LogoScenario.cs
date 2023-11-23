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
        // 활성화 하지 않은상태에서도 게임 진행

        Application.runInBackground = true;

        //해상도 설정(9:18.5 , 1440ㅌ2960)
        int width = Screen.width;
        int height = (int)(Screen.width*18.5f/9);
        Screen.SetResolution(width, height, true);

        //화면이 꺼지지않도록
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        progress.Play(OnAfterProgress);
    }

    private void OnAfterProgress()
    {
        Utils.LoadScene(nextScene);
    }
}
