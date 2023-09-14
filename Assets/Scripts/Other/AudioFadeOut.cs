using System.Collections;
using UnityEngine;

public class AudioFadeOut : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public float fadeDuration = 2.0f; // Duration of the fade in seconds
    public float delayBeforeFade = 5.0f; // Delay before starting the fade in seconds

    private float startVolume; // Initial volume of the audio source

    private void Start()
    {
        if (audioSource == null)
        {
            // If audioSource is not assigned, try to find one on the same GameObject
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            // Record the initial volume of the audio source
            startVolume = audioSource.volume;

            // Start the delay timer before fading out
            StartCoroutine(StartDelay());
        }
        else
        {
            Debug.LogWarning("AudioFadeOut: No AudioSource found.");
        }
    }

    private IEnumerator StartDelay()
    {
        // Wait for the specified delay before starting the fade-out
        yield return new WaitForSeconds(delayBeforeFade);

        // Start the fade out coroutine
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // Calculate the new volume based on the elapsed time
            float newVolume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);

            // Set the audio source volume to the new volume
            audioSource.volume = newVolume;

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the volume is completely zero at the end
        audioSource.volume = 0f;

        // Optionally, you can stop the audio playback here
        audioSource.Stop();
    }
}
