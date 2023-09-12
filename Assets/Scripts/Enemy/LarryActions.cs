using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LarryActions : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private float increaseFactor = 1.1f;

    [SerializeField]
    private LarryState currentState;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private AudioClip stunAudio;

    [SerializeField]
    private AudioClip attackAudio1;

    [SerializeField]
    private AudioClip attackAudio2;

    [SerializeField]
    private AudioClip laughingAudio1;

    [SerializeField]
    private AudioClip laughingAudio2;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Camera playerCam;

    private AudioSource audioSource;
    private Animator animator;
    private NavMeshAgent ai;

    private List<Transform> destinations;
    private Vector3 currentDestination;
    private bool ready;
    private bool final;
    private float chaseTime;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ai = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        destinations = new List<Transform>();
        ready = false;
        final = false;
        currentState = LarryState.Idle;
        chaseTime = 10f;
        InvokeRepeating("killPlayer", 10.0f, 3.0f);
    }

    private void FixedUpdate()
    {
        if (ready == true) 
        {
            switch (currentState) 
            {
                case LarryState.Idle:
                    ai.speed = 0;
                    IdleActions();
                    break;
                case LarryState.Patrol:
                    ai.speed = speed * increaseFactor;
                    PatrolActions();
                    break;
                case LarryState.Chase:
                    ChaseActions();
                    ai.speed = speed * increaseFactor * 1.2f;
                    break;
                case LarryState.Stun:
                    ai.speed = 0;
                    break;
                case LarryState.Attack:
                    ai.speed = 0;
                    break;
                case LarryState.Kill:
                    Debug.Log("Kill");
                    ai.speed = speed * increaseFactor * 1.4f;
                    break;
            }
        }
    }

    public void killPlayer() 
    {
        PlayerMotor pm = player.GetComponent<PlayerMotor>();
        if (pm.numKeys == 4 && currentState != LarryState.Killing) 
        {
            currentState = LarryState.Kill;
            final = true;
        }
    }


    public void setRandomDestinations(List<Transform> destinations) 
    {
        this.destinations = destinations;
        StartCoroutine(start());
    }

    private Vector3 getNextDestination() 
    {
        Vector3 result = new Vector3(0,0,0);
        float distance = 0;

        for (int i = 0; i < destinations.Count; i++) 
        {
            if (Vector3.Distance(destinations[i].position, transform.position) > distance) 
            {
                result = destinations[i].position;
                distance = Vector3.Distance(destinations[i].position, transform.position);
            }
        }

        return result;
    }
    
    private void IdleActions() 
    {
        animator.Play("Larry_Idle2");
        if (final == true)
        {
            currentState = LarryState.Kill;
        }
        else 
        {
            currentState = LarryState.Patrol;
        }
    }

    private void PatrolActions() 
    {
        animator.Play("Larry_Walk1");
        currentDestination = getNextDestination();
        ai.destination = currentDestination;
        currentState = LarryState.Walking;
    }

    private void ChaseActions() 
    {
        ai.destination = player.transform.position;
        animator.Play("Larry_Run");
        StopCoroutine(chasing());
        StartCoroutine(chasing());
        currentState = LarryState.Chasing;
    }

    public void StunActions() 
    {
        animator.Play("Larry_Stun3");
        audioSource.PlayOneShot(stunAudio);
        StopCoroutine(chasing());
        StartCoroutine(recover(10f));
    }

    public void AttackActions(string attack) 
    {
        playerCam.gameObject.SetActive(false);
        cam.gameObject.SetActive(true);
        animator.Play(attack);
        audioSource.PlayOneShot(Random.Range(0, 2) == 0 ? attackAudio1 : attackAudio2);
        StopCoroutine(chasing());
        StartCoroutine(recover(10f));
    }

    public void KillActions() 
    {
        ai.destination = player.transform.position;
        animator.Play("Larry_Run");
        currentState = LarryState.Killing;
    }

    public void recoverPlayerCam()
    {
        cam.gameObject.SetActive(false);
        playerCam.gameObject.SetActive(true);
        player.GetComponent<PlayerMotor>().playerAttacked();
    }

    IEnumerator start() 
    {
        animator.Play("Larry_JumpScare1");
        yield return new WaitForSeconds(5f);
        ready = true;
    }

    IEnumerator recover(float seconds) 
    {
        ai.speed = 0;
        yield return new WaitForSeconds(seconds);
        currentState = LarryState.Idle;
    }

    IEnumerator chasing() 
    {
        yield return new WaitForSeconds(chaseTime);
        if (Vector3.Distance(transform.position, player.transform.position) > 5) 
        {
            currentState = LarryState.Idle;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            audioSource.PlayOneShot(Random.Range(0, 2) == 0 ? laughingAudio1 : laughingAudio2);
            currentState = LarryState.Chase;
        }

        if (other.gameObject.CompareTag("destination"))
        {
            ai.speed = 0;
            animator.Play("Larry_Celebration1");
            StartCoroutine(recover(5f));
        }
    }
}

public enum LarryState
{
    Idle,
    Patrol,
    Stun,
    Chase,
    Attack,
    Walking,
    Chasing,
    Kill,
    Killing
}
