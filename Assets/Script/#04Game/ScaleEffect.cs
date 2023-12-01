using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScaleEffect : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10f)]
    private float effectTime; // 크기 확대/축소 되는시간
    private TextMeshProUGUI effectText; //확대축소에 사용되는 텍스트

    private void Awake()
    {
        effectText = GetComponent<TextMeshProUGUI>();
    }

    public void Play(float start, float end)
    {
        StartCoroutine(Process(start, end));
    }

    private IEnumerator Process(float start, float end)
    {
        float current = 0;
        float percent = 0;
        while (percent < 1) 
        {
            current += Time.deltaTime;
            percent = current / effectTime;

            effectText.fontSize = Mathf.Lerp(start, end, percent);

            yield return null;
                
        }
    }
}
