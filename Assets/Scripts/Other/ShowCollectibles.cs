using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCollectibles : MonoBehaviour
{
    public GameObject ticket;
    public GameObject newspaper;
    public GameObject missingperson;
    public GameObject drawing1;
    public GameObject drawing2;
    public GameObject drawing3;
    public GameObject drawing4;
    public GameObject drawing5;
    public GameObject invite1;
    public GameObject invite2;
    public GameObject invite3;
    public GameObject polaroid1;
    public GameObject polaroid2;
    public GameObject polaroid3;
    public GameObject polaroid4;
    public GameObject polaroid6;
    public GameObject LinkNT;
    public GameObject LinkD3P4;
    public GameObject LinkD5P6;
    public GameObject LinkMI2;
    public GameObject LinkD2I1;
    public GameObject LinkD2I2;
    public GameObject LinkD2I3;
    public GameObject LinkND1;
    public GameObject LinkNM;
    public GameObject LinkNP2;

    // Start is called before the first frame update
    void Start()
    {
        LinkNT.SetActive(false);
        LinkD3P4.SetActive(false);
        LinkD5P6.SetActive(false);
        LinkMI2.SetActive(false);
        LinkD2I1.SetActive(false);
        LinkD2I2.SetActive(false);
        LinkD2I3.SetActive(false);
        LinkND1.SetActive(false);
        LinkNM.SetActive(false);
        LinkNP2.SetActive(false);
        ticket.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("ticket");
        if (PlayerPrefsManager.LoadBool("ticket")) {Destroy(GameObject.Find("ticket (1)"));}
        newspaper.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("newspaper");
        if (PlayerPrefsManager.LoadBool("newspaper")) {Destroy(GameObject.Find("Newspaper (1)"));}
        missingperson.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("missingperson");
        if (PlayerPrefsManager.LoadBool("missingperson")) {Destroy(GameObject.Find("MissingPoster (1)"));}
        drawing1.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("drawing1");
        if (PlayerPrefsManager.LoadBool("drawing1")) {Destroy(GameObject.Find("Drawing1 (1)"));}
        drawing2.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("drawing2");
        if (PlayerPrefsManager.LoadBool("drawing2")) {Destroy(GameObject.Find("Drawing2 (1)"));}
        drawing3.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("drawing3");
        if (PlayerPrefsManager.LoadBool("drawing3")) {Destroy(GameObject.Find("Drawing3 (1)"));}
        drawing4.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("drawing4");
        if (PlayerPrefsManager.LoadBool("drawing4")) {Destroy(GameObject.Find("Drawing4 (1)"));}
        drawing5.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("drawing5");
        if (PlayerPrefsManager.LoadBool("drawing5")) {Destroy(GameObject.Find("Drawing5 (1)"));}
        invite1.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("invite1");
        if (PlayerPrefsManager.LoadBool("invite1")) {Destroy(GameObject.Find("Invitation1 (1)"));}
        invite2.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("invite2");
        if (PlayerPrefsManager.LoadBool("invite2")) {Destroy(GameObject.Find("Invitation2 (1)"));}
        invite3.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("invite3");
        if (PlayerPrefsManager.LoadBool("invite3")) {Destroy(GameObject.Find("Invitation3 (1)"));}
        polaroid1.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("polaroid1");
        if (PlayerPrefsManager.LoadBool("polaroid1")) {Destroy(GameObject.Find("Polaroid (1)"));}
        polaroid2.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("polaroid2");
        if (PlayerPrefsManager.LoadBool("polaroid2")) {Destroy(GameObject.Find("Polaroid2 (1)"));}
        polaroid3.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("polaroid3");
        if (PlayerPrefsManager.LoadBool("polaroid3")) {Destroy(GameObject.Find("Polaroid3 (1)"));}
        polaroid4.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("polaroid4");
        if (PlayerPrefsManager.LoadBool("polaroid4")) {Destroy(GameObject.Find("Polaroid4 (1)"));}
        polaroid6.GetComponent<Renderer>().enabled = PlayerPrefsManager.LoadBool("polaroid6");
        if (PlayerPrefsManager.LoadBool("polaroid6")) {Destroy(GameObject.Find("Polaroid6 (1)"));}
        if (PlayerPrefsManager.LoadBool("newspaper")&&PlayerPrefsManager.LoadBool("ticket")) {LinkNT.SetActive(true);}
        if (PlayerPrefsManager.LoadBool("drawing3")&&PlayerPrefsManager.LoadBool("polaroid4")) {LinkD3P4.SetActive(true);;}
        if (PlayerPrefsManager.LoadBool("drawing5")&&PlayerPrefsManager.LoadBool("polaroid6")) {LinkD5P6.SetActive(true);;}
        if (PlayerPrefsManager.LoadBool("missingperson")&&PlayerPrefsManager.LoadBool("invite2")) {LinkMI2.SetActive(true);;}
        if (PlayerPrefsManager.LoadBool("drawing2")&&PlayerPrefsManager.LoadBool("invite1")) {LinkD2I1.SetActive(true);;}
        if (PlayerPrefsManager.LoadBool("drawing2")&&PlayerPrefsManager.LoadBool("invite2")) {LinkD2I2.SetActive(true);;}
        if (PlayerPrefsManager.LoadBool("drawing2")&&PlayerPrefsManager.LoadBool("invite3")) {LinkD2I3.SetActive(true);;}
        if (PlayerPrefsManager.LoadBool("newspaper")&&PlayerPrefsManager.LoadBool("drawing1")) {LinkND1.SetActive(true);;}
        if (PlayerPrefsManager.LoadBool("newspaper")&&PlayerPrefsManager.LoadBool("missingperson")) {LinkNM.SetActive(true);;}
        if (PlayerPrefsManager.LoadBool("newspaper")&&PlayerPrefsManager.LoadBool("polaroid2")) {LinkNP2.SetActive(true);;}
    }
}
