using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
public class LoginSample : MonoBehaviour
{
    private void Awake()
    {
        string ID = "ASD1";
        string PW = "1234";
        string email = "asd@gmail.com";
        string nickname = "����1";

        //ȸ������
        Backend.BMember.CustomSignUp(ID, PW);

        //�̸��� ����
        Backend.BMember.UpdateCustomEmail(email);

        //�α���
        Backend.BMember.CustomLogin(ID, PW);

        //���̵� ã��
        Backend.BMember.FindCustomID(email);

        //��й�ȣã��
        Backend.BMember.ResetPassword(ID, email);

        //�г��Ӽ���
        //�г��� ������ ���ʼ���
        Backend.BMember.CreateNickname(nickname);
        //�̹� �ִ� �г��� ���� (������ creat����)
        Backend.BMember.UpdateNickname(nickname);
    }
}
