using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeInAndOut : MonoBehaviour
{
    public bool damage = false;
    public float fadeDuration = 1.0f;

    public Image guiImage;

    private bool isFadingIn = false;
    private bool isFadingOut = false;
    private float timer = 0.0f;

    private void Start()
    {
        SetAlpha(0);
    }

    private void Update()
    {
        if (damage)
        {
            if (!isFadingIn && !isFadingOut && guiImage.color.a < 1)
            {
                isFadingIn = true;
                StartCoroutine(Fade(1));
            }
        }
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = guiImage.color.a;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            SetAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetAlpha(targetAlpha);

        if (targetAlpha == 1)
        {
            isFadingIn = false;
            isFadingOut = true;
            StartCoroutine(Fade(0));
        }
        else
        {
            isFadingOut = false;
            damage = false;
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = guiImage.color;
        color.a = alpha;
        guiImage.color = color;
    }
}
