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
    public LarryState currentState;

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
        chaseTime = 2f;
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
                    ai.speed = speed * increaseFactor * 1.3f;
                    break;
                case LarryState.Stun:
                    ai.speed = 0;
                    break;
                case LarryState.Attack:
                    ai.speed = 0;
                    break;
                case LarryState.Kill:
                    ai.speed = speed * increaseFactor * 1.6f;
                    KillActions();
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
        if (currentState != LarryState.Stun)
        {
            animator.Play("Larry_Walk1");
            currentDestination = getNextDestination();
            ai.destination = currentDestination;
            currentState = LarryState.Walking;
        }
    }

    private void ChaseActions() 
    {
        if (currentState != LarryState.Stun) 
        {
            ai.destination = player.transform.position;
            animator.Play("Larry_Run");
            StopCoroutine(chasing());
            StartCoroutine(chasing());
            currentState = LarryState.Chasing;
        }
    }

    public void StunActions() 
    {
        if (currentState != LarryState.Stun) 
        {
            currentState = LarryState.Stun;
            animator.Play("Larry_Stun3");
            audioSource.PlayOneShot(stunAudio);
            StopAllCoroutines();
            StartCoroutine(recover(10f, false));
        }
    }

    public void AttackActions(string attack) 
    {
        if (player.GetComponent<PlayerMotor>().hit == false && currentState != LarryState.Stun) 
        {
            player.GetComponent<PlayerMotor>().hit = true;
            playerCam.gameObject.SetActive(false);
            cam.gameObject.SetActive(true);
            StopAllCoroutines();
            animator.Play(attack);
            audioSource.PlayOneShot(Random.Range(0, 2) == 0 ? attackAudio1 : attackAudio2);
            StartCoroutine(recover(2f, true));
        }
    }

    public void KillActions() 
    {
        if (currentState != LarryState.Stun) 
        {
            ai.destination = player.transform.position;
            animator.Play("Larry_Run");
            currentState = LarryState.Killing;
            StopCoroutine(comeBackToKill());
            StartCoroutine(comeBackToKill());
        }
    }

    IEnumerator comeBackToKill() 
    {
        yield return new WaitForSeconds(2f);
        currentState = LarryState.Kill;
    }

    public void recoverPlayerCam()
    {
        cam.gameObject.SetActive(false);
        playerCam.gameObject.SetActive(true);
        player.GetComponent<PlayerMotor>().playerAttacked();
    }

    IEnumerator start() 
    {
        if (animator == null) 
        {
            Debug.Log("animator null");
            animator = GetComponent<Animator>();
        }
        
        animator.Play("Larry_JumpScare1");
        yield return new WaitForSeconds(5f);
        ready = true;
    }

    IEnumerator recover(float seconds, bool fromKill) 
    {
        ai.speed = 0;
        yield return new WaitForSeconds(seconds);
        if (fromKill && playerCam.gameObject.activeSelf == false)
        {
            recoverPlayerCam();
        }
        currentState = LarryState.Idle;
    }

    IEnumerator chasing() 
    {
        yield return new WaitForSeconds(chaseTime);
        Debug.Log("distance enemy player in chasing " + Vector3.Distance(transform.position, player.transform.position));
        if (Vector3.Distance(transform.position, player.transform.position) > 5)
        {
            currentState = LarryState.Idle;
        }
        else 
        {
            currentState = LarryState.Chase;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            audioSource.PlayOneShot(Random.Range(0, 2) == 0 ? laughingAudio1 : laughingAudio2);
            if(currentState != LarryState.Kill && currentState != LarryState.Killing && currentState != LarryState.Stun)
                currentState = LarryState.Chase;
        }

        if (other.gameObject.CompareTag("destination"))
        {
            ai.speed = 0;
            animator.Play("Larry_Celebration1");
            StartCoroutine(recover(3f, false));
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
