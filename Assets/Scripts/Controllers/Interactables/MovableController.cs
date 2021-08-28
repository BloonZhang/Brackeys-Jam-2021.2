using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableController : Interactable
{
    // setting variables
    public Vector3 moveDistance;

    // definition variables
    private float timeToMove = 0.5f;
    private float movesPerSecond = 30f; // essentially FPS

    // helper variables
    private bool alreadyMoved = false;
    public bool canMoveBack = true;
    private bool locked = false;

    public override void OnClick()
    {
        // Don't do anything if not active
        if (!active) { return; }
        // Don't respond if locked
        if (locked) { return; }

        // Move
        if (!alreadyMoved) 
        { 
            StartCoroutine(Move(moveDistance));
            alreadyMoved = true; 
        }
        // Move back
        else if (alreadyMoved && canMoveBack) 
        { 
            StartCoroutine(Move(-moveDistance)); 
            alreadyMoved = false; 
        }
    }

    IEnumerator Move(Vector3 distance)
    {
        locked = true;
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
        locked = false;
    }

}
