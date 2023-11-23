using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : MonoBehaviour
{
    public void BtnClickGameStart()
    {
        Utils.LoadScene(SceneNames.Game);
    }
}
