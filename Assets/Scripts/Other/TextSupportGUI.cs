using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSupportGUI : MonoBehaviour
{
    public bool showText;
    private string headerMessage;
    private string subInteractionMessage;
    private string interactionMessage;
    private string quote;

    private void Start()
    {
        showText = true;
        headerMessage = "";
        subInteractionMessage = "";
        interactionMessage = "";
        quote = "";
    }

    public void setHeaderMessage(string msg) 
    {
        headerMessage = msg;
    }

    public void setSubInteractionMessage(string msg) 
    {
        subInteractionMessage = msg;
        StartCoroutine(cleanSubMsg());
    }

    public void setInteractionMessage(string msg, bool isQuote) 
    {
        interactionMessage = msg;

        if (isQuote) 
        {
            quote = msg;
            StartCoroutine(cleanQuote());
        }
    }

    public void cleanMessages() 
    {
        interactionMessage = quote;
        headerMessage = "";
    }

    IEnumerator cleanQuote() 
    {
        yield return new WaitForSeconds(5f);
        quote = "";
    }

    IEnumerator cleanSubMsg() 
    {
        yield return new WaitForSeconds(3f);
        subInteractionMessage = "";
    }

    private void OnGUI()
    {
        if (!showText) return;

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, Screen.height * 0.08f, Screen.width, Screen.height * 0.15f), "<color=white><size=80>" + headerMessage + "</size></color>", style);
        GUI.Label(new Rect(0, Screen.height * 0.8f, Screen.width, Screen.height * 0.25f), "<color=white><size=50>" + subInteractionMessage + "</size></color>", style);
        GUI.Label(new Rect(0, Screen.height * 0.8f, Screen.width, Screen.height * 0.15f), "<color=white><size=50>" + interactionMessage + "</size></color>", style);
    }
}
