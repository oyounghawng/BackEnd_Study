using System.Collections;
using TMPro;
using UnityEngine;
public class FadeEffect_TMP : MonoBehaviour
{
    [SerializeField]
    private float effectTime = 1.5f;
    private TextMeshProUGUI effectText;

    private void Awake()
    {
        effectText = GetComponent<TextMeshProUGUI>();

        //처음 알파값을 0으로 설정
        Color color = effectText.color;
        color.a = 0;
        effectText.color = color;
    }

    public void FadeOut(string text)
    {
        effectText.text = text;
        StartCoroutine(OnFade(1, 0));
    }

    public IEnumerator OnFade(float start, float end)
    {
        float currnet = 0;
        float percent = 0;

        while (percent < 1)
        {
            currnet += Time.deltaTime;
            percent = currnet / effectTime;

            Color color = effectText.color;
            color.a = Mathf.Lerp(start, end, percent);
            effectText.color = color;

            yield return null;
        }
    }
}
