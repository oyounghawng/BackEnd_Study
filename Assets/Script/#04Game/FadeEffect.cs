using System.Collections;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float fadeTime = 0.1f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(nameof(TwinkleLoop));
    }
    private IEnumerator TwinkleLoop()
    {
        while(true)
        {
            //알파값을 1에서 0으로 fade out
            yield return Onfade(1,0);

            //알파값을 0에서 1로 fade in
            yield return Onfade(0,1);
        }
    }

    private IEnumerator Onfade(float start,float end)
    {
        float currnet = 0;
        float percent = 0;

        while(percent<1)
        {
            currnet += Time.deltaTime;
            percent = currnet / fadeTime;

            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(start, end, percent);
            spriteRenderer.color = color;

            yield return null;
        }
    }
}
