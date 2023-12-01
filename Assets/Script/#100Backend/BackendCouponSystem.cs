using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BackEnd;
using UnityEngine.UIElements;
public class BackendCouponSystem : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputFieldCode;
    [SerializeField]
    private FadeEffect_TMP textResult;

    public void ReceiveCoupon()
    {
        string couponCode = inputFieldCode.text;

        if(couponCode.Trim().Equals(""))
        {
            textResult.FadeOut("���� �ڵ带 �Է��� �ּ���.");
            return;
        }

        inputFieldCode.text = "";

        ReceiveCoupon(couponCode);
    }

    public void ReceiveCoupon(string couponCode)
    {
        Backend.Coupon.UseCoupon(couponCode, callback =>
        {
            if( !callback.IsSuccess())
            {
                //���� �ޱ⿡ ���� ���� �� ó��
                FailedToReceive(callback);
                return;
            }
            
            //json ������ �Ľ�
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["itemObject"];

                //�޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
                if(jsonData.Count <= 0 )
                {
                    Debug.LogWarning("������ �������� �����ϴ�.");
                    return;
                }

                //������ �ִ� ��� ������ ����
                SaveToLocal(jsonData);
            }
            catch (System.Exception e)
            {
                //t-c�� ���� ���
                Debug.LogError(e);
            }
        });
    }

    public void FailedToReceive(BackendReturnObject callback)
    {
        if (callback.GetMessage().Contains("���� ����")) 
        {
            textResult.FadeOut("���� ���� ������ �����Ǿ��ų� �Ⱓ�� ����� �����Դϴ�.");
        }
        else if(callback.GetMessage().Contains("�̹� ����Ͻ� ����"))
        {
            textResult.FadeOut("�ش� ������ �̹� ����ϼ̽��ϴ�.");
        }
        else
        {
            textResult.FadeOut("���� �ڵ尡 �߸��Ǿ��ų� �̹� ����� �����Դϴ�.");
        }
        Debug.LogError($"���� ��� �� ������ �߻��߽��ϴ�. : {callback}");
    }
    private void SaveToLocal(LitJson.JsonData items)
    {
        //json ������ �Ľ̼���
        try
        {
            string getItems = string.Empty;

            //������ �ִ� ��� ������ ����
            foreach(LitJson.JsonData item in items)
            {
                int itemId = int.Parse(item["item"]["itemId"].ToString());
                string itemName = item["item"]["itemName"].ToString();
                string itemInfo = item["item"]["itemInfo"].ToString();
                int itemCount = int.Parse(item["itemCount"].ToString());

                if (itemName.Equals("heart")) BackendGameData.Instance.UserGameData.heart += itemCount;
                else if (itemName.Equals("gold")) BackendGameData.Instance.UserGameData.gold += itemCount;
                else if (itemName.Equals("jewel")) BackendGameData.Instance.UserGameData.jewel += itemCount;

                getItems += $"{itemName} : {itemCount}";
            }

            textResult.FadeOut($"���� ������� ������{getItems}�� ȹ���߽��ϴ�.");

            //�÷��̾� ��ȭ ���� ������Ʈ
            BackendGameData.Instance.GameDataUpdate();
        }
        //�Ľ� ����
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

}
