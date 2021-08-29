using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController : Interactable
{
    /* // Inhereted
    public string tag = "DefaultInteractable";
    public bool active = true;
    public virtual void Triggered();
    */

    // jank code, but the mana crystal's collider2d seems to block other colliders even when it's behind them??
    // TODO: figure out why this is happening
    private PolygonCollider2D collider;
    void Start() { collider = GetComponent<PolygonCollider2D>(); }
    void Update()
    {
        if (!victoryEnabled) { collider.enabled = false; }
        else if (victoryEnabled) { collider.enabled = true; }
    }

    public override void OnClick()
    {
        if (victoryEnabled){ LevelManager.Instance.Win(); }
    }

    // static methods
    // TODO: make not static in case multiple win conditions
    public static bool victoryEnabled = false;
    public static void Activate() { victoryEnabled = true; }
    public static void Deactivate() { victoryEnabled = false; }

}
