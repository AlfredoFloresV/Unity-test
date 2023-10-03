using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMessages : MonoBehaviour
{
    [SerializeField]
    private GameObject textObj;

    private int msgState;
    private Dictionary<int, string> messages;

    // Start is called before the first frame update
    void Start()
    {
        messages = new Dictionary<int, string>();
        messages[0] = "";
        messages[1] = "I should find a way inside...";
        messages[2] = "It's locked. I need to find something to open the doors";
        messages[3] = "If only I could buy a <b>ticket</b>";
        messages[4] = "What?";
        messages[5] = "I should take a closer look at that <b>arrow</b>";
        msgState = 0;
        textObj.GetComponent<TextSupportGUI>().cleanMessages();
        startIntro();
    }

    private void startIntro() 
    {
        textObj.GetComponent<TextSupportGUI>().cleanMessages();

        if (msgState < 4) 
        {
            textObj.GetComponent<TextSupportGUI>().setInteractionMessage(messages[msgState], true);
            StartCoroutine(waiting(true));
        }
    }

    public void continueIntro() 
    {
        StopCoroutine(waiting(true));
        msgState = msgState < 4 ? 4 : msgState;
        
        if (msgState < 6) 
        {
            textObj.GetComponent<TextSupportGUI>().setInteractionMessage(messages[msgState], true);
            StartCoroutine(waiting(false));
        }
    }

    IEnumerator waiting(bool part1) 
    {
        yield return new WaitForSeconds(7f);
        msgState++;
        if (part1) startIntro();
        else continueIntro();
    }
}
