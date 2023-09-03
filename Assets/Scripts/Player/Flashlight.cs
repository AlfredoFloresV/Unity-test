using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField]
    private Light spotlight;


    [SerializeField]
    private Transform flEnabled;
    [SerializeField]
    private Transform flDisabled;
    [SerializeField]
    private AudioClip clickBtn;

    private Light l;
    private Rigidbody rb;   
    private AudioSource audioSource;
    private float movementStep = 5f;
    private bool disabled = true;

    // Start is called before the first frame update
    void Start()
    {
        l = spotlight.GetComponent<Light>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1)) 
        {
            audioSource.PlayOneShot(clickBtn);
            StartCoroutine(WaitForClickBtn());
            //l.enabled = !l.enabled;
        }
        /*
        if (l.enabled)
        {
            Vector3 currPos = transform.position;
            Vector3 targetPos = flEnabled.position;
            Vector3 dir = new Vector3(0f, 0f, 1f);
            float distance = Vector3.Distance(currPos, targetPos);
            Debug.Log(distance);
            if (distance > 0.1)
            {
                rb.MovePosition(currPos + (dir * movementStep * Time.fixedDeltaTime));
            }
        }
        else
        {
            Vector3 currPos = transform.position;
            Vector3 targetPos = flDisabled.position;
            Vector3 dir = new Vector3(0f, 0f, -1f);
            float distance = Vector3.Distance(currPos, targetPos);
            Debug.Log(distance);
            if (distance < 0.1)
            {
                rb.MovePosition(currPos + (dir * movementStep * Time.fixedDeltaTime));
            }
        }
        */
    }

    IEnumerator WaitForClickBtn()
    {
        yield return new WaitForSeconds(1);
        l.enabled = !l.enabled;
    }

}
