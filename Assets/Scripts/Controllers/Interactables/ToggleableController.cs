using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleableController : Interactable
{
    /* // Inhereted
    public string tag = "DefaultInteractable";
    public bool active = true;
    */

    // Components
    private SpriteRenderer renderer;
    private PolygonCollider2D collider;

    // setting variables
    private Sprite originalSprite;
    public Sprite toggledSprite;
    public PolygonCollider2D originalCollider;
    public PolygonCollider2D toggledCollider;

    // helper variables
    private bool toggled = false;
    public bool canToggleBack = true;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<PolygonCollider2D>();
        originalSprite = renderer.sprite;
        collider.points = originalCollider.points;
    }

    public override void OnClick()
    {
        // Don't do anything if not active
        if (!active) { return; }
        
        // Toggle
        if (!toggled) 
        { 
            renderer.sprite = toggledSprite; 
            collider.points = toggledCollider.points;
            toggled = true; 
        }
        // Toggle back
        else if (toggled && canToggleBack) 
        { 
            renderer.sprite = originalSprite;
            collider.points = originalCollider.points;
            toggled = false; 
        }
    }

}
