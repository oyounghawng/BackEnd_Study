using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    //스테이지 크기 상하 좌우
    [SerializeField]
    private Vector2 limitMin;
    [SerializeField]
    private Vector2 limitMax;

    //다른 클래스에서는 limitmin limit max 프로퍼티를 통해 크기 확인가능

    public Vector2 LimitMin => limitMin;
    public Vector2 LimitMax => limitMax;
}
