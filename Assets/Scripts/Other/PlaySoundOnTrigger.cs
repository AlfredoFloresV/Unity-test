using UnityEngine;

public class PlaySoundOnTrigger : MonoBehaviour
{
    public AudioClip soundClip; // The sound clip to play
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        // Get a reference to the AudioSource component on the same GameObject
        audioSource = GetComponent<AudioSource>();

        // Make sure an AudioSource component exists, and assign the audio clip
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on this GameObject.");
        }
        else
        {
            audioSource.clip = soundClip;
        }
    }

    // Called when another Collider enters this trigger collider
    void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is tagged as "Player"
        if (other.CompareTag("player"))
        {
            // Play the assigned sound clip
            audioSource.Play();
        }
    }
}
