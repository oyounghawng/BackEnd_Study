using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
public class Nickname : LoginBase
{
    [System.Serializable]
    public class NicknameEvent : UnityEngine.Events.UnityEvent { }

    public NicknameEvent onNicknameEvent = new NicknameEvent();

    [SerializeField]
    private Image imageNickname;  //�г��� �ʵ� ���󺯰�
    [SerializeField]
    private TMP_InputField inputFieldNickname; //�Է��ʵ�

    [SerializeField]
    private Button btnUpdateNickname; // �г��� ���� ��ư

    private void OnEnable()
    {
        //���濡 ������ �����޼��� ����ѻ��¿���
        //�ݾҴ� ���� �ֱ� ������ ���� �ʱ�ȭ
        ResetUI(imageNickname);
        SetMessage("�г����� �Է��ϼ���");

    }

    public void OnClickUpdateNickName()
    {
        //�Ű����� �Է��� ��ǲ�ʵ�� �޽��� �ʱ�ȭ
        ResetUI(imageNickname);

        //������� üũ
        if (IsFieldDataEmpty(imageNickname, inputFieldNickname.text, "Nickname")) return;

        //��ȣ�ۿ� ��Ȱ��ȭ(�ߺ����� ����)
        btnUpdateNickname.interactable = false;
        SetMessage("�г��� ���� ���Դϴ�..");

        //�ڳ����� ����õ�
        UpdateNickname();
    }

    private void UpdateNickname()
    {
        //�г��� ����
        Backend.BMember.UpdateNickname(inputFieldNickname.text, callback =>
        {
            //��ư Ȱ��ȭ
            btnUpdateNickname.interactable = true;

            //���� ����
            if(callback.IsSuccess())
            {
                SetMessage($"{inputFieldNickname.text}(��)�� �г����� ����Ǿ����ϴ�.");

                //�̺�Ʈ �޼ҵ� ���
                onNicknameEvent?.Invoke();
            }
            //����
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 400: // ������ �ֲ��� 20���̻� ���ѻ��׿� �ɸ� �г���
                        message = "�г����� ����ų�, 20���̻��̰ų�, ��/�ڿ� ������ �ֽ��ϴ�.";
                        break;
                    case 409: //�ߺ��� �г���
                        message = "�̹� �����ϴ� �г��� �Դϴ�.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }
            }
        });
    }
}
