using UnityEngine;

public class AudioSwitch : MonoBehaviour
{
    public bool switchAudio = false; // Boolean to control the audio switch
    public AudioClip originalAudioClip; // The original audio clip
    public AudioClip alternateAudioClip; // The alternate audio clip
    private AudioSource audioSource; // Reference to the AudioSource component

    private void Start()
    {
        // Get a reference to the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Set the initial audio clip to the original one
        audioSource.clip = originalAudioClip;

        // Play the audio
        audioSource.Play();
    }

    private void Update()
    {
        // Check the value of the switchAudio bool
        if (switchAudio)
        {
            // If it's true, switch to the alternate audio clip
            audioSource.clip = alternateAudioClip;
        }
        else
        {
            // If it's false, switch back to the original audio clip
            audioSource.clip = originalAudioClip;
        }

        // Reset the bool to prevent continuous switching (you can adjust this based on your needs)
        switchAudio = false;
    }
}
