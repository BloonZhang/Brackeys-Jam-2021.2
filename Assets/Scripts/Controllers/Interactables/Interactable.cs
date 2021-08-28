using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    public string tag = "DefaultInteractable";
    public bool active = true;

    void OnMouseUpAsButton()
    {
        OnClick();
    }

    public abstract void OnClick();

}
