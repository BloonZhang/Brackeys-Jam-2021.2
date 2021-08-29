using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helper class to ensure nested lists in Unity inspector
[Serializable]
public class ForStageList
{
    // Constructor
    public ForStageList() { m_forStageList = new List<Interactable>(); }
    public ForStageList(List<Interactable> list) { m_forStageList = list; }

    // fields
    public List<Interactable> m_forStageList;

    // user-defined conversions
    public static implicit operator List<Interactable>(ForStageList list)
    {
        return list.m_forStageList;
    }
    public static implicit operator ForStageList(List<Interactable> list)
    {
        return new ForStageList(list);
    }
}