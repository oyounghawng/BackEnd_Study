using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
using System;
public class Post : MonoBehaviour
{
    [SerializeField]
    private Sprite[] spriteItemIcons;  //우편에 포함된 아이템 아이콘에 출력할 이미지 배열
    [SerializeField]
    private Image imageItemIcon; // 우편에 포함된 아이템 아이콘 출력
    [SerializeField]
    private TextMeshProUGUI textItemCount; // 우편에 포함된 아이템의 개수
    [SerializeField]
    private TextMeshProUGUI textTitle; //우편 제목
    [SerializeField]
    private TextMeshProUGUI textContent; //우편 내용
    [SerializeField]
    private TextMeshProUGUI textExpirationDate; // 우편 만료까지 남은 시간 출력

    [SerializeField]
    private Button buttonReceive; //우편 수령버튼

    private BackendPostSystem backendPostSystem;
    private PopupPostBox popupPostBox;
    private PostData postData;

    public void Setup(BackendPostSystem postSystem, PopupPostBox postBox, PostData postData)
    {
        //우편 수령 버튼을 눌렀을때 처리
        buttonReceive.onClick.AddListener(OnClickPostReceive);

        backendPostSystem = postSystem;
        popupPostBox = postBox;
        this.postData   = postData;

        //우편 제목과 내용 설정
        textTitle.text = postData.title;
        textContent.text = postData.content;

        //첫번째 아이템 정보를 우편에 출력
        foreach( string itemKey in postData.postReward.Keys)
        {
            //우편에 포함된 아이템 이미지 출력
            if (itemKey.Equals("heart")) imageItemIcon.sprite = spriteItemIcons[0];
            else if (itemKey.Equals("gold")) imageItemIcon.sprite = spriteItemIcons[1];
            else if (itemKey.Equals("jewel")) imageItemIcon.sprite = spriteItemIcons[2];

            //우편에 포함된 아이템 개수 출력
            textItemCount.text = postData.postReward[itemKey].ToString();

            //하나의 우편에 포함된 아이템이 여러개 일 수도 있는데 현재 예제에서는 첫번쨰 아이템 정보만 출력
            break;
        }

        //getservertime() 서버시간 호출
        Backend.Utils.GetServerTime(callback =>
        {
            if(!callback.IsSuccess())
            {
                Debug.LogError($"서버시간 불러오기에 실패했습니다. : {callback}");
                return;
            }

            //json 데이터 파싱성공
            try
            {
                //현제 서버 시간
                string serverTime = callback.GetFlattenJSON()["utcTime"].ToString();

                //우편 만료까지 남은 시간 = 우편만료 시간 - 현재 서버 시간
                TimeSpan timeSpan = DateTime.Parse(postData.expirationDate) - DateTime.Parse(serverTime);

                //timespan.totalhours로 남은시간을 시간단위로 표현
                textExpirationDate.text = $"{timeSpan.TotalHours:F0}시간 후 만료";

            }
            //JSON 데이터 파싱 실패
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        });
    }

    private void OnClickPostReceive()
    {
        //현재 우편 ui오브제그 삭제
        popupPostBox.DestroyPost(gameObject);
        //우편 수령
        backendPostSystem.PostReceive(PostType.Admin, postData.inDate);
    }
}
