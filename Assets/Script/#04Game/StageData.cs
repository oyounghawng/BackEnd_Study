using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    //�������� ũ�� ���� �¿�
    [SerializeField]
    private Vector2 limitMin;
    [SerializeField]
    private Vector2 limitMax;

    //�ٸ� Ŭ���������� limitmin limit max ������Ƽ�� ���� ũ�� Ȯ�ΰ���

    public Vector2 LimitMin => limitMin;
    public Vector2 LimitMax => limitMax;
}
