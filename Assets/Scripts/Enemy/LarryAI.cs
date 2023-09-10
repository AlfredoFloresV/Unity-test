using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI;

public class LarryAI : MonoBehaviour
{
    [SerializeField]
    private int Speed = 2;

    [SerializeField]
    private float chaseTime = 10;

    [SerializeField]
    private float idleTime = 3;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private AudioClip laffying1;

    [SerializeField]
    private AudioClip laffying2;

    [SerializeField]
    private AudioClip attack1;

    [SerializeField]
    private AudioClip attack2;

    [SerializeField]
    private AudioClip hurt;

    [SerializeField]
    private LayerMask mask;

    private NavMeshAgent ai;
    private Animator animator;
    private int randDecision;



    private AudioSource audioSource;
    private List<Transform> destinations;
    private List<string> attacks;
    public bool walking, chasing, idle; //attacking, stunned
    private Vector3 dest;
    private bool startActions;


    public bool stun = false;

    private void Start()
    {
        walking = false;
        chasing = false;
        idle = true;
        stun = false;

        animator = GetComponent<Animator>();
        destinations = new List<Transform>();
        ai = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        startActions = false;
        attacks = new List<string>() { "Larry_Attack1", "Larry_Attack2", "Larry_Attack3" };
        //Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), player.GetComponent<CapsuleCollider>(), );
    }

    public void setRandomDestinations(List<Transform> dest)
    {
        destinations = dest;
        walking = true;
        StartCoroutine(larryIntro());
    }

    public void patrol()
    {
        animator.Play("Larry_Walk1"); //checar
        randDecision = Random.Range(0, destinations.Count);

        dest = destinations[randDecision].position;
    }

    private void Update()
    {
        if (startActions == true && stun == false)
        {
            if (chasing == true)
            {
                Debug.Log("chasing");
                walking = false;
                dest = new Vector3(player.transform.position.x, 0, player.transform.position.z);
                ai.destination = dest;
                ai.speed = Speed * 2f;
                animator.Play("Larry_Run");
                StopCoroutine(nextDestination());
                StopCoroutine(chase());
                StartCoroutine(chase());
            }
            else if (walking == true && destinations.Count > 0)
            {
                Debug.Log("patrol");
                patrol();
                ai.destination = dest;
                ai.speed = Speed;
                walking = false;
            }
            else if (idle == true)
            {
                Debug.Log("idle");
                ai.speed = 0;
                StartCoroutine(nextDestination());
                idle = false;
            }

            //detectPlayer();
            //StartCoroutine(ValidateLoc());
        }
    }

    /*
    private void detectPlayer()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position - new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward), out hit, 5.5f, mask))
        {
            if (hit.collider.tag == "player")
            {
                chasing = true;
                walking = false;
                idle = false;
                StopCoroutine(nextDestination());
                StopCoroutine(chase());
                StartCoroutine(chase());
            }
        }

        if (Physics.Raycast(transform.position - new Vector3(0,0.5f,0), transform.TransformDirection(Vector3.back), out hit, 3.5f, mask))
        {
            if (hit.collider.tag == "player")
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
                chasing = true;
                walking = false;
                idle = false;
                StopCoroutine(nextDestination());
                StopCoroutine(chase());
                StartCoroutine(chase());
            }
        }
    }*/


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            animator.Play(attacks[Random.Range(0, attacks.Count)]);
            audioSource.PlayOneShot(Random.Range(0, 2) == 0 ? attack1 : attack2);
            
            chasing = false;
            walking = false;
            idle = false;
            GetComponent<CapsuleCollider>().enabled = false;
            
            StopCoroutine(chase());
            StopCoroutine(nextDestination());
            StartCoroutine(attack());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            chasing = true;
            walking = false;
            idle = false;
            StopCoroutine(nextDestination());
            StopCoroutine(chase());
            StartCoroutine(chase());
        }

        if (other.gameObject.CompareTag("destination"))
        {
            if (chasing == false)
            {
                walking = false;
                idle = true;
                animator.Play("Larry_Celebration1");
            }
        }
    }



    public IEnumerator nextDestination()
    {
        yield return new WaitForSeconds(idleTime);
        walking = true;
        chasing = false;
        idle = false;
    }

    public IEnumerator chase()
    {
        yield return new WaitForSeconds(chaseTime);
        chasing = false;
        walking = true;
        idle = false;
    }

    IEnumerator larryIntro()
    {
        Debug.Log("Larry intro");
        animator.Play("Larry_JumpScare1");
        yield return new WaitForSeconds(5f);
        startActions = true;
    }

    IEnumerator attack()
    {
        idle = true;
        yield return new WaitForSeconds(15f);
        GetComponent<CapsuleCollider>().enabled = true;
        audioSource.PlayOneShot(Random.Range(0, 2) == 0 ? laffying1 : laffying2);
    }

    public IEnumerator recover()
    {
        yield return new WaitForSeconds(20f);
        stun = false;
    }

    public IEnumerator ValidateLoc() 
    {
        Vector3 loc = transform.position;
        yield return new WaitForSeconds(10f);
        Vector3 loc2 = transform.position;

        if (Vector3.Distance(loc, loc2) < 0.5) 
        {
            walking = true;
            idle = false;
            chasing = false;
            Debug.Log("again");
        }
    }

}
