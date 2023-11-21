using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BackEnd;
using UnityEngine.UI;
public class RegisterAccount : LoginBase
{
    /// <summary>
    /// image : �̹��� ������ inpufield : �Է¶�
    /// </summary>
    [SerializeField]
    private Image imageID;
    [SerializeField]
    private TMP_InputField inputFieldID;
    [SerializeField]
    private Image imagePW;
    [SerializeField]
    private TMP_InputField inputFieldPW;
    [SerializeField]
    private Image imageConfirmPW;
    [SerializeField]
    private TMP_InputField inputFieldConfirmPW;
    [SerializeField]
    private Image imageEmail;
    [SerializeField]
    private TMP_InputField inputFieldEmail;

    [SerializeField]
    private Button btnRegisterAccount;

    /// <summary>
    /// �������� ��ư�� �������� ȣ��
    /// </summary>
    public void OnClickRegisterAccount()
    {
        //�Ű������� �ν��� ��ǲ�ʵ��� ����� �޽��� ���� �ʱ�ȭ
        ResetUI(imageID,imagePW,imageConfirmPW,imageEmail);

        //������� üũ
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "��й�ȣ")) return;
        if (IsFieldDataEmpty(imageConfirmPW, inputFieldConfirmPW.text, "��й�ȣ Ȯ��")) return;
        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "���� �ּ�")) return;

        //��й�ȣ�� ��й�ȣ Ȯ���� �ٸ���
        if(!inputFieldPW.text.Equals(inputFieldConfirmPW.text))
        {
            GuideForIncorrectlyEnterData(imageConfirmPW, "��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
            return;
        }

        //���� ���İ˻�
        if (!inputFieldPW.text.Contains("@"))
        {
            GuideForIncorrectlyEnterData(imageEmail, "���������� �߸��Ǿ����ϴ�. (ex. address@xx.xx) ");
            return;
        }

        //���� ���� ��ư�� ��ȣ�ۿ� ��Ȱ��ȭ
        btnRegisterAccount.interactable = false;
        SetMessage("���� �������Դϴ�.");

        //�څ� ���� ���Ļ��� �õ�
        CustomSingUp();
    }

    /// <summary>
    /// ���� ���� �õ��� ������ ���
    /// </summary>
    private void CustomSingUp()
    {
        Backend.BMember.CustomSignUp(inputFieldID.text, inputFieldPW.text, callback =>
        {
            //�������� ��ư Ȱ��ȭ
            btnRegisterAccount.interactable = true;

            if(callback.IsSuccess())
            {
                Backend.BMember.UpdateCustomEmail(inputFieldEmail.text, callback =>
                {
                    if (callback.IsSuccess())
                    {
                        SetMessage($"���� ���� ����.{inputFieldEmail}");

                        //�κ� �� �̵�
                        Utils.LoadScene(SceneNames.Lobby);
                    }
                });
            }
            //���� ���� ����
            else
            {
                string message = string.Empty;

                switch(int.Parse(callback.GetStatusCode())) 
                {
                    case 409: // �ߺ��� ���̵�
                        message = "�̹� �����ϴ� ���̵� �Դϴ�.";
                        break;
                    case 403: //���ܴ��� ���̵�

                        break;
                    case 401://������Ʈ ��밡 '����'��

                        break;
                    case 400://����̽� ������ null

                        break;
                    default:
                        message = callback.GetMessage();
                        break;

                }

                if(message.Contains("���̵�"))
                {
                    GuideForIncorrectlyEnterData(imageID, message);
                }
                else
                {
                    SetMessage(message);
                }
            }
        });
        
    }

}
