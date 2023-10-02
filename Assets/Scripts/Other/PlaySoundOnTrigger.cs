using UnityEngine;

public class PlaySoundOnTrigger : MonoBehaviour
{
    public AudioClip soundClip; // The sound clip to play
    private AudioSource audioSource; // Reference to the AudioSource component
    private GameObject[] enemies;

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

        enemies = GameObject.FindGameObjectsWithTag("mainenemy");
        Debug.Log("enemies found: " + enemies.Length);
    }

    // Called when another Collider enters this trigger collider
    void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is tagged as "Player"
        if (other.CompareTag("player"))
        {
            // Play the assigned sound clip
            audioSource.Play();

            GameObject enemy = Random.Range(0, 2) == 0 ? enemies[0] : enemies[1];
            enemy.GetComponent<LarryActions>().currentState = LarryState.Horn;
            enemy.GetComponent<LarryActions>().HornActions(transform.position);
        }
    }
}
