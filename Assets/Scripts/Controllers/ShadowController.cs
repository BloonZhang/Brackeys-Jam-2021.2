using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// basic script that makes sure shadows always are the same transparency as their parent
public class ShadowController : MonoBehaviour
{
    private SpriteRenderer parentSpriteRenderer;

    void Start()
    {
        parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color = parentSpriteRenderer.color;
    }
}
