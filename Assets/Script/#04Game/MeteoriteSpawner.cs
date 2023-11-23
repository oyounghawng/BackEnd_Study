using System.Collections;
using UnityEngine;

public class MeteoriteSpawner : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private StageData stageData;
    [SerializeField]
    private GameObject alertLinePrefab;
    [SerializeField]
    private GameObject meteoritePrefab;
    [SerializeField]
    private float minSpawnCyleTime = 1; //최소 생성주기
    [SerializeField]
    private float maxSpawnCyleTime = 4; //최대 생성주기

    private void Awake()
    {
        StartCoroutine(nameof(Process));
    }

    private IEnumerator Process()
    {
        while (true)
        {
            //대기 시간 설정
            float spawnCycleTime = Random.Range(minSpawnCyleTime,maxSpawnCyleTime);
            //spawn 시간동안 대기
            yield return new WaitForSeconds(spawnCycleTime);

            //경고선 운석이 생성되는 위치는 범위 내 x축 랜덤
            float x = Random.Range(stageData.LimitMin.x, stageData.LimitMax.x);

            //경고선 오브젝트 생성
            GameObject alertLineClone = Instantiate(alertLinePrefab, new Vector3(x,0,0),Quaternion.identity);

            //1초 대기후
            yield return new WaitForSeconds(1f);

            //경고선 오브젝트 삭제
            Destroy(alertLineClone);

            //운석 오브젝트 생성(y위치는 스테이지 상단 위치 +1)
            GameObject meteorite = Instantiate(meteoritePrefab, new Vector3(x, stageData.LimitMax.y + 1, 0), Quaternion.identity);
            meteorite.GetComponent<Meteorite>().Setup(gameController);
        }
    }
}
