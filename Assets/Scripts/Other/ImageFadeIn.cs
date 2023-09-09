using UnityEngine;
using UnityEngine.UI;

public class ImageFadeIn : MonoBehaviour
{
    public Image imageToFade; // Reference to the Image component to fade in
    public float fadeInTime = 2.0f; // Duration of the fade-in in seconds
    public float startDelay = 1.0f; // Delay before starting the fade-in

    private float startTime;
    private bool isFading = false;

    private void Start()
    {
        // Ensure the Image component is not visible initially
        imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 0f);
        startTime = Time.time + startDelay;
    }

    private void Update()
    {
        if (Time.time >= startTime && !isFading)
        {
            isFading = true;
            StartCoroutine(FadeInImage());
        }
    }

    private IEnumerator FadeInImage()
    {
        float elapsedTime = 0f;
        Color startColor = imageToFade.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsedTime < fadeInTime)
        {
            float t = elapsedTime / fadeInTime;
            imageToFade.color = Color.Lerp(startColor, endColor, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the image is fully opaque
        imageToFade.color = endColor;
    }
}
