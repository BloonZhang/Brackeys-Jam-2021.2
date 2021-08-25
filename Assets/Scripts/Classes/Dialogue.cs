using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   // allows class to be changed in inspector
public class Dialogue
{
    //public string name;
    [TextArea(3, 10)]   // for inspector ease of access
    public string[] sentences;
}
