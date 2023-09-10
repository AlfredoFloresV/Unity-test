using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Add this using directive for IEnumerator


public class ContinueButton : MonoBehaviour
{
    public Image imageToFade; // Reference to the Image component to fade in
    public Button startButton; // Reference to the StartButton
    public float fadeInTime = 2.0f; // Duration of the fade-in in seconds
    public float startDelay = 1.0f; // Delay before starting the fade-in

    private float startTime;
    private bool isFading = false;

    void Start()
    {
        startButton.interactable = false; // Disable the button initially
        imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 0f);
        startTime = Time.time + startDelay;
    }

    void Update()
    {
        if (Time.time >= startTime && !isFading)
        {
            isFading = true;
            StartCoroutine(FadeInImage());
        }
    }

    IEnumerator FadeInImage()
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

        imageToFade.color = endColor;
        startButton.interactable = true; // Enable the button after fading in
    }
}
