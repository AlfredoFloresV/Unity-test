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

    private NavMeshAgent ai;
    private Animator animator;
    private int randDecision;



    private AudioSource audioSource;
    private List<Transform> destinations;
    private List<string> attacks;
    private bool walking, chasing, idle; //attacking, stunned
    private Vector3 dest;
    private bool startActions;


    private void Start()
    {
        walking = false;
        chasing = false;
        idle = true;
        
        animator = GetComponent<Animator>();
        destinations = new List<Transform>();
        ai = GetComponent<NavMeshAgent>();
        startActions = false;
        attacks = new List<string>(){ "Larry_Attack1", "Larry_Attack2", "Larry_Attack3" };
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
        if (startActions == true)
        {
            if (chasing == true)
            {
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
                //Debug.Log("patrol");
                patrol();
                ai.destination = dest;
                ai.speed = Speed;
                walking = false;
            }
            else if (idle == true)
            {
                //Debug.Log("idle");
                ai.speed = 0;
                StartCoroutine(nextDestination());
                idle = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            chasing = true;
            walking = false;
            idle = false;
            //animator.ResetTrigger("idle");
            //animator.ResetTrigger("walk");
            StopCoroutine(nextDestination());
            StopCoroutine(chase());
            StartCoroutine(chase());
        }

        if (other.CompareTag("destination"))
        {
            if (chasing == false)
            {
                walking = false;
                idle = true;
                animator.Play("Larry_Celebration1");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player")) 
        {
            chasing = false;
            walking = false;
            idle = false;
            GetComponent<BoxCollider>().enabled = false;
            //player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            StopCoroutine(chase());
            StopCoroutine(nextDestination());
            StartCoroutine(attack());
            
        }
    }


    IEnumerator nextDestination()
    {
        yield return new WaitForSeconds(idleTime);
        walking = true;
        chasing = false;
        idle = false;
    }

    IEnumerator chase()
    {
        yield return new WaitForSeconds(chaseTime);
        chasing = false;
        walking = true;
        idle = false;
    }

    IEnumerator larryIntro() 
    {
        Debug.Log("Larry intro");
        animator.Play("Larry_Intro");
        yield return new WaitForSeconds(5f);
        startActions = true;
    }

    IEnumerator attack() 
    {
        idle = true;
        animator.Play(attacks[Random.Range(0, attacks.Count)]);
        yield return new WaitForSeconds(10f);
        GetComponent<BoxCollider>().enabled = true;
        //player.GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePosition;
        //player.gameObject.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionY;
    }
}
