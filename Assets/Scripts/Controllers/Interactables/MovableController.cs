using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableController : Interactable
{
    /* // Inhereted
    public string tag = "DefaultInteractable";
    public bool active = true;
    public virtual void Triggered();
    public virtual void SetFlag();
    public virtual void ResetFlag()
    */

    // setting variables
    public Vector3 moveDistance;

    // definition variables
    private float timeToMove = 2.5f;
    private float movesPerSecond = 30f; // essentially FPS

    // helper variables
    private bool alreadyMoved = false;
    public bool canMoveBack = true;
    private bool selfLocked = false;

    public override void OnClick()
    {
        // Don't respond if selfLocked
        if (selfLocked) { return; }

        // Move
        if (!alreadyMoved) 
        { 
            alreadyMoved = true; 
            GlobalCoroutineList.Add(StartCoroutine(Move(moveDistance)));
        }
        // Move back
        else if (alreadyMoved && canMoveBack) 
        {
            alreadyMoved = false; 
            GlobalCoroutineList.Add(StartCoroutine(Move(-moveDistance))); 
        }
    }

    IEnumerator Move(Vector3 distance)
    {
        selfLocked = true;
        Vector3 destination = transform.position + distance;
        float timer = 0f;
        float timeIncrement = 1f / movesPerSecond;
        while (timer < timeToMove)
        {
            transform.position += (distance * timeIncrement) / timeToMove;
            yield return new WaitForSeconds(timeIncrement);
            timer += timeIncrement;
        }
        transform.position = destination;
        selfLocked = false;

        // If the alreadyMoved flag is true, then we were moving it initially, so call Triggered() and SetFlag()
        if (alreadyMoved) { Triggered(); SetFlag(); }
        else { ResetFlag(); }
    }

}
