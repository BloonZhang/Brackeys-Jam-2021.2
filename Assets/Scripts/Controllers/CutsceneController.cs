using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public Dialogue dialogue;

    void Start()
    {
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        DialogueController.Instance.StartDialogue(dialogue);
    }
}
