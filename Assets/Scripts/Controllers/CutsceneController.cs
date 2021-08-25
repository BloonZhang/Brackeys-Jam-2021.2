using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{

    //////// Singleton shenanigans ////////
    private static CutsceneController _instance;
    public static CutsceneController Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    public Dialogue dialogue;

    public SpriteRenderer jahy;
    public Sprite jahyProud;
    public Sprite jahySweat;
    public Sprite jahyAngry;
    public SpriteRenderer apartment;

    // definition variables
    float fadeTime = 1.5f;

    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}
    }

    void Start()
    {
        // Set up by turning all sprite renderers invisible
        Vector4 transparent = new Vector4(1f, 1f, 1f, 0f);
        jahy.color = transparent;
        apartment.color = transparent;
        StartCoroutine(Begin());
    }

/*
    void Update()
    {
        if (Input.GetKeyDown("a")) { ToAngry(); }
    }
*/

    // public methods
    public void ToSweat() { FadeTo(jahy, jahySweat); }
    public void ToAngry() { SwitchTo(jahy, jahyAngry); }
    public void ToApartment() { FadeTo(apartment, apartment.sprite); }

    // Methods
    void TriggerDialogue()
    {
        DialogueController.Instance.StartDialogue(dialogue);
    }
    void FadeTo(SpriteRenderer renderer, Sprite newSprite)
    {
        // Create a copy of this gameobject that will be destroyed later
        SpriteRenderer tempRenderer = Instantiate(renderer.gameObject).GetComponent<SpriteRenderer>();
        tempRenderer.sortingOrder = renderer.sortingOrder - 1;
        // Switch the actual gameobject to the new sprite, but set alpha to 0
        SwitchTo(renderer, newSprite); renderer.color = new Vector4(1f, 1f, 1f, 0);
        // Fade in actual renderer, fade out temporary one
        StartCoroutine(FadeIn(renderer)); 
        StartCoroutine(DeleteAfter(tempRenderer, fadeTime + 1f));
    }
    void SwitchTo(SpriteRenderer renderer, Sprite newSprite)
    {
        renderer.sprite = newSprite;
    }

    // Coroutines
    IEnumerator Begin()
    {
        jahy.color = new Vector4(1f, 1f, 1f, 0); jahy.sprite = jahyProud;
        while (jahy.GetComponent<SpriteRenderer>().color.a < 1f)
        {
            Color newColor = jahy.GetComponent<SpriteRenderer>().color;
            newColor.a = Mathf.Min(newColor.a + (5f/255f), 1f);
            jahy.GetComponent<SpriteRenderer>().color = newColor;
            yield return new WaitForSeconds( (5f * fadeTime) / 255f );
        }
        TriggerDialogue();
    }
    IEnumerator FadeOut(SpriteRenderer renderer)
    {
        while (renderer.color.a > 0)
        {
            Color newColor = renderer.color;
            newColor.a = Mathf.Max(newColor.a - (5f/255f), 0f);
            renderer.color = newColor;
            yield return new WaitForSeconds( (5f * fadeTime) / 255f );
        }
    }
    IEnumerator FadeIn(SpriteRenderer renderer)
    {
        while (renderer.color.a < 1f)
        {
            Color newColor = renderer.color;
            newColor.a = Mathf.Min(newColor.a + (5f/255f), 1f);
            renderer.color = newColor;
            yield return new WaitForSeconds( (5f * fadeTime) / 255f );
        }
    }
    IEnumerator DeleteAfter(SpriteRenderer renderer, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(renderer.gameObject);
    }
}
