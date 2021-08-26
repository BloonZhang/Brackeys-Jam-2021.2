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
    public Animator jahyAnimator;

    // helper variables
    private bool dialogueStarted = false;
    private bool dialogueLock = false;
    private bool mouseDown = false;

    // definition variables
    private float timeBetweenLetters = 0.02f;

    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}
    }

    // Start is called before the first frame update
    void Start()
    {
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
        // Debug.Log(currentSentence);

        // special sentences here.
        // Ex. pause:500
        if (currentSentenceArray.Length > 1)
        {
            // Split up the special word and the argument
            int argument = -1;
            try { argument = int.Parse(currentSentenceArray[1]); }
            catch (FormatException) { Debug.Log("Error in DialogueController.ContinueDialogue()"); }

            switch (currentSentenceArray[0])
            {
                case "pause":
                    PauseDialogue(argument, false);
                    break;
                case "pauseautostart":
                    PauseDialogue(argument, true);
                    break;
                case "animation":
                    if (argument == 0) 
                    { 
                        CutsceneController.Instance.ToSweat(); 
                        CutsceneController.Instance.ToApartment();
                    }
                    if (argument == 1) { CutsceneController.Instance.ToAngry(); }
                    if (argument == 2) { CutsceneController.Instance.ToLandlady(); }
                    if (argument == 3) { Debug.Log("here"); CutsceneController.Instance.ToTextBubble(); }
                    ContinueDialogue();
                    break;
                case "dialoguebox":
                    if (argument == 0) { CloseDialogueBox(); }
                    if (argument == 1) { OpenDialogueBox(); }
                    ContinueDialogue();
                    break;
                default:
                    Debug.Log("reached default case for DialogueController.ContinueDialogue(). Probably a typo");
                    break;
            }
        }
        // Normal sentences here
        else
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentSentence));
        }
    }

    void OpenDialogueBox() { dialogueAnimator.SetBool("IsOpen", true); }
    void CloseDialogueBox() { dialogueAnimator.SetBool("IsOpen", false); }
    void PauseDialogue(int time, bool autoStartNext)
    {
        StartCoroutine(Pause(time, autoStartNext));
    }
    void EndDialogue()
    {
        //Debug.Log("Ending dialogue");
        CloseDialogueBox();
    }

    // Coroutines
    IEnumerator TypeSentence(string sentence)
    {
        dialogueBoxText.text = "";
        dialogueLock = true;
        jahyAnimator.SetBool("talking", true);
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueBoxText.text += letter;
            yield return new WaitForSeconds(timeBetweenLetters);
        }
        jahyAnimator.SetBool("talking", false);
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
