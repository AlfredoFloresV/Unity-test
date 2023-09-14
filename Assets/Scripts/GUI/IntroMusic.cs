using UnityEngine;

public class IntroMusic : MonoBehaviour
{
    public AudioSource[] audioSources;
    private int currentAudioIndex = 0;

    public float initialDelay = 2.0f; // Adjust this value for the initial delay in seconds
    private float currentDelay = 0.0f;
    private bool hasStarted = false;

    // Singleton instance
    private static IntroMusic instance;

    void Awake()
    {
        // Ensure only one instance of this script exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy any additional instances
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Check if the initial delay has passed
        if (!hasStarted)
        {
            currentDelay += Time.deltaTime;

            if (currentDelay >= initialDelay)
            {
                hasStarted = true;
                PlayNextAudio(); // Start playing audio after the initial delay
            }
        }

        // Check if the current audio source has finished playing
        if (hasStarted && audioSources.Length > 0 && !audioSources[currentAudioIndex].isPlaying)
        {
            // Move to the next audio source
            currentAudioIndex++;

            // Check if all audio sources have been played
            if (currentAudioIndex < audioSources.Length)
            {
                // Play the next audio source
                PlayNextAudio();
            }
            else
            {
                // All audio sources have been played
                Debug.Log("All audio sources have been played.");

                // Loop the last audio source
                currentAudioIndex = audioSources.Length - 1;
                PlayNextAudio();
            }
        }
    }

    void PlayNextAudio()
    {
        if (currentAudioIndex < audioSources.Length)
        {
            // Play the current audio source
            audioSources[currentAudioIndex].Play();
        }
    }
}
