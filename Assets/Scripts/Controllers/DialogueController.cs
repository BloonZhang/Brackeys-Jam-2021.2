using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{

    //////// Singleton shenanigans ////////
    private static DialogueController _instance;
    public static DialogueController Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    private Queue<string> sentences;

    // public variables
    public TextMeshProUGUI dialogueBoxText;
    public Animator dialogueAnimator;

    // helper variables
    private bool dialogueStarted = false;
    private bool dialogueLock = false;
    private bool mouseDown = false;

    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueAnimator.SetBool("IsOpen", true);
        sentences = new Queue<string>();
    }

    void Update()
    {
        // Check for clicking to continue dialogue
        if (dialogueStarted) {
            if (Input.GetMouseButtonDown(0) && !dialogueLock) { mouseDown = true; }
            if (Input.GetMouseButtonUp(0)) 
            { 
                if (!dialogueLock && mouseDown) { ContinueDialogue(); }
                mouseDown = false;
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //Debug.Log("Starting dialogue");
        // Clear out previous dialogue
        sentences.Clear();
        // populate sentences
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        dialogueStarted = true;
        ContinueDialogue();
    }

    public void ContinueDialogue()
    {
        if (sentences.Count == 0) { EndDialogue(); return; }

        string currentSentence = sentences.Dequeue();
        string[] currentSentenceArray = currentSentence.Split(':');

        // special sentences here.
        // Ex. pause:500
        if (currentSentenceArray.Length > 1)
        {
            // Split up the special word and the argument
            string argument = currentSentenceArray[1];

            switch (currentSentenceArray[0])
            {
                case "pause":
                    try { PauseDialogue(int.Parse(argument), false); }
                    catch (FormatException) { Debug.Log("Error in DialogueController.ContinueDialogue() - pause"); }
                    break;
                case "pauseautostart":
                    try { PauseDialogue(int.Parse(argument), true); }
                    catch (FormatException) { Debug.Log("Error in DialogueController.ContinueDialogue() - pauseautostart"); }
                    break;
                default:
                    break;
            }
        }
        // Normal sentences here
        else
        {
            // StopAllCoroutines();
            StartCoroutine(TypeSentence(currentSentence));
        }
    }

    void PauseDialogue(int time, bool autoStartNext)
    {
        StartCoroutine(Pause(time, autoStartNext));
    }

    void EndDialogue()
    {
        //Debug.Log("Ending dialogue");
        dialogueAnimator.SetBool("IsOpen", false);
    }

    // Coroutines
    IEnumerator TypeSentence(string sentence)
    {
        dialogueBoxText.text = "";
        dialogueLock = true;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueBoxText.text += letter;
            yield return null;  // one letter per frame
        }
        dialogueLock = false;
    }
    IEnumerator Pause(int time, bool autoStartNext)
    {
        //Debug.Log("Dialogue paused");
        dialogueLock = true;
        yield return new WaitForSeconds((float)time / 1000f);
        //Debug.Log("Dialogue continued");
        dialogueLock = false;
        if (autoStartNext) { ContinueDialogue(); }
    }
}
