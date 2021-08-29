using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Static
    private static bool masterLock = false;
    public static void LockInteractables() { masterLock = true; }
    public static void UnlockInteractables() { masterLock = false; }

    public string tag = "DefaultInteractable";
    public bool active = true;

    // when clicked
    void OnMouseUpAsButton()
    {
        // Don't do anything if the master lock is on
        if (masterLock) { return; }
        // Don't do anything if not active
        if (!active) { return; }
        OnClick();
    }

    // abstract methods
    public abstract void OnClick();
    // virtual methods
    public virtual void Triggered()
    {
        GameOverManager.Instance.Triggered(tag);
    }
    public virtual void SetFlag() { LevelManager.Instance.SetFlag(tag); }
    public virtual void ResetFlag() { LevelManager.Instance.ResetFlag(tag); }

}
