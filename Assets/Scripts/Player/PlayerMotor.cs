using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;


public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject cameraEffects;

    [SerializeField]
    private GameObject flashLight;

    [SerializeField]
    private AudioClip hurt;

    [SerializeField]
    private AudioClip boo;

    [SerializeField]
    private AudioClip locked;

    [SerializeField]
    private AudioClip unlock;

    [SerializeField]
    private AudioClip claps;

    [SerializeField]
    private GameObject fade;

    [SerializeField]
    private GameObject hearbeat;

    [SerializeField]
    private GameObject bgmusic;

    [SerializeField]
    private GameObject siren;

    [SerializeField]
    private GameObject textObj;

    private AudioSource audioSource;

    public int Health = 5;
    public int Damage = 0;
    
    public Dictionary<string, bool> keysFound;
    public bool victory;
    public string VictoryKey;
    public int numKeys;
    public bool hit;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        keysFound = new Dictionary<string, bool>();

        for (int i = 1; i < 5; i++) 
            keysFound["key" + i] = false;
        
        Random random = new Random();

        VictoryKey = "" + (random.Next(1, 5));
        //Debug.Log("VictoryKey" + VictoryKey);
        numKeys = 0;
        hit = false;
        victory = false;
        if(hearbeat != null) hearbeat.SetActive(Health - Damage <= 1);
    }

    public void playerAttacked() 
    {
        audioSource.PlayOneShot(hurt);
        cam.GetComponent<Animator>().Play("ShakeCamera");
        cameraEffects.GetComponent<UIFadeInAndOut>().damage = true;
        handleLife();
        hit = false;
    }

    private void handleLife() 
    {
        Damage += 1;
        if (Health - Damage >= 0)
        {
            cameraEffects.GetComponent<UIMaterialSwitcher>().damageScreen = Damage;
            hearbeat.SetActive(Health - Damage <= 1);
        }
        else 
        {
            //Actualizar!!
            SceneManager.LoadScene("GameOver1 1");
        }
    }

    public void restoreHealth() 
    {
        Health = Health + 1 >= 5 ? 5 : Health + 1;
        Damage = Damage - 1 <= 0 ? 0 : Damage - 1;
        cameraEffects.GetComponent<UIMaterialSwitcher>().damageScreen = Damage;
        hearbeat.SetActive(Health - Damage <= 1);
    }

    public void restoreLight() 
    {
        flashLight.GetComponent<Flashlight>().chargeBattery();
    }

    public void spendLight() 
    {
        flashLight.GetComponent<Flashlight>().spendLight(0.15f);
    }

    public float getIntensity() 
    {
        return flashLight.GetComponent<Flashlight>().intensity;
    }

    public bool getFocus() 
    {
        return flashLight.GetComponent<Flashlight>().focus;
    }

    public bool lightEnabled() 
    {
        return flashLight.GetComponent<Flashlight>().lightEnabled();
    }

    public void handleKeys(string key) 
    {
        keysFound[key] = true;
        numKeys++;
        if (numKeys == 4) 
        {
            siren.SetActive(true);
            bgmusic.GetComponent<AudioSwitch>().switchAudio = true;
            textObj.GetComponent<TextSupportGUI>().setInteractionMessage("I gotta get outta here!", true);
        }
    }

    public void verifyVictoryCondition(string doorNum, GameObject doorObj) 
    {
        bool isCorrectDoor = doorNum == VictoryKey;

        if (keysFound["key" + doorNum] == true)
        {
            audioSource.PlayOneShot(unlock);
            doorObj.tag = "specialdoorused";
            
            if (isCorrectDoor)
            {
                victory = true;
                StartCoroutine(win(doorObj, claps, true));
            }
            else
            {
                StartCoroutine(win(doorObj, boo, false));
            }
        }
        else
        {
            audioSource.PlayOneShot(locked);
        }
    }

    IEnumerator win(GameObject go, AudioClip audio, bool w) 
    {
        yield return new WaitForSeconds(4f);
        go.GetComponent<Animator>().Play("open");
        audioSource.PlayOneShot(audio);
        if (w)
        {
            hearbeat.SetActive(false);
            bgmusic.SetActive(false);
            fade.SetActive(true);
            fade.GetComponent<Animator>().Play("FadeOut");
            StartCoroutine(winGame());
        }
        else 
        {
            Destroy(go.transform.GetChild(1).gameObject);
        }
    }

    IEnumerator winGame() 
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("Win");
    }

}
