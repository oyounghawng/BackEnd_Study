using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private StageData stageData;

    private Movement2D movement2D;
    private Weapon weapon;
    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        weapon = GetComponent<Weapon>();
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        movement2D.MoveDirection = new Vector3(x, y, 0);

        //공격 주기 계산하고 가능하면 공격하기
        weapon.WeaponAction();
    }

    private void LateUpdate()
    {
        //화면 밖으로 나가지 못하도록 제어
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x)
                                        ,Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y)
                                        ,0);

    }
}
