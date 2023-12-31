using System.Collections;
using UnityEngine;
using TMPro;
public class CountingEffect : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10f)]
    private float effectTime;
    private TextMeshProUGUI effectText;

    private void Awake()
    {
        effectText = GetComponent<TextMeshProUGUI>();
    }

    public void Play(int start,int end)
    {
        StartCoroutine(Porcess(start, end));
    }
    private IEnumerator Porcess(int start,int end)
    {
        float current = 0;
        float percent = 0;
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / effectTime;

            effectText.text = Mathf.Lerp(start, end, percent).ToString("F0");

            yield return null;

        }
    }
}
