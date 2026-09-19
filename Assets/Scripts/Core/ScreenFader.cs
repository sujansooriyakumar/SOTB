using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader instance;
    [SerializeField] private Image screenFade;

    public UnityEvent OnFadeComplete;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        if (screenFade != null)
        {
            Color c = screenFade.color;
            c.a = 0f;
            screenFade.color = c;
        }
    }

    public void BeginScreenFade(float duration)
    {
        if(screenFade != null)
        {
            StartCoroutine(FadeScreen(duration));
        }
    }

    private IEnumerator FadeScreen(float duration)
    {
        float timer = 0f;

        SetAlpha(1f);
        yield return new WaitForSeconds(duration / 2);
        while (timer < duration/2)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / (duration / 2));
            SetAlpha(alpha);

            timer += Time.deltaTime;
            yield return null;
        }

        SetAlpha(0f);
        OnFadeComplete?.Invoke();
    }

    private void SetAlpha(float alpha)
    {
        if(screenFade != null)
        {
            Color c = screenFade.color;
            c.a = alpha;
            screenFade.color = c;
        }
    }
}
