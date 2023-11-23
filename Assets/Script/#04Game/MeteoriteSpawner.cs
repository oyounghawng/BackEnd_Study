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
    private float minSpawnCyleTime = 1; //�ּ� �����ֱ�
    [SerializeField]
    private float maxSpawnCyleTime = 4; //�ִ� �����ֱ�

    private void Awake()
    {
        StartCoroutine(nameof(Process));
    }

    private IEnumerator Process()
    {
        while (true)
        {
            //��� �ð� ����
            float spawnCycleTime = Random.Range(minSpawnCyleTime,maxSpawnCyleTime);
            //spawn �ð����� ���
            yield return new WaitForSeconds(spawnCycleTime);

            //��� ��� �����Ǵ� ��ġ�� ���� �� x�� ����
            float x = Random.Range(stageData.LimitMin.x, stageData.LimitMax.x);

            //��� ������Ʈ ����
            GameObject alertLineClone = Instantiate(alertLinePrefab, new Vector3(x,0,0),Quaternion.identity);

            //1�� �����
            yield return new WaitForSeconds(1f);

            //��� ������Ʈ ����
            Destroy(alertLineClone);

            //� ������Ʈ ����(y��ġ�� �������� ��� ��ġ +1)
            GameObject meteorite = Instantiate(meteoritePrefab, new Vector3(x, stageData.LimitMax.y + 1, 0), Quaternion.identity);
            meteorite.GetComponent<Meteorite>().Setup(gameController);
        }
    }
}
