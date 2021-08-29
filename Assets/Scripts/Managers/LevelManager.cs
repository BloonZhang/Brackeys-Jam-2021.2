using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //////// Singleton shenanigans ////////
    private static LevelManager _instance;
    public static LevelManager Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    // public varaibles
    public int currentLevelNumber = -1;
    public List<ForStageList> victoryConditions;

    // helper variables
    public Dictionary<string, bool> victoryFlagsForStage = new Dictionary<string, bool>();

    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}

        // Keep this around between scenes
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SetLevel(1);
    }

    // helper methods
    void SetLevel(int level)
    {
        Interactable.UnlockInteractables();
        currentLevelNumber = level;
        ResetDictionary(level);
    }
    void ResetDictionary(int level)
    {
        // Set up the victory condition flags
        List<Interactable> victoryConditionsForStage = new List<Interactable>();
        try { victoryConditionsForStage = victoryConditions[level - 1]; }
        catch (Exception e) { Debug.Log("Error in LevelManager.ResetDictionary()"); }
        foreach(Interactable interactable in victoryConditionsForStage)
        {
            victoryFlagsForStage.Add(interactable.tag, false);
        }
    }
    void StopAllCoroutinesGlobally()
    {
        foreach (Coroutine cor in GlobalCoroutineList.GetList())
        {
            Debug.Log("stopping coroutine");
            StopCoroutine(cor);
        }
    }

    // public methods
    public void SetFlag(string tag)
    {
        // Set flag
        if (victoryFlagsForStage.ContainsKey(tag)) 
        { 
            victoryFlagsForStage[tag] = true; 
            // if all flags are true, ready to win
            if (!victoryFlagsForStage.ContainsValue(false)) { WinController.Activate(); }
        }
    }
    public void ResetFlag(string tag)
    {
        // Reset flag
        if (victoryFlagsForStage.ContainsKey(tag))
        {
            victoryFlagsForStage[tag] = false;
            // not ready to win
            WinController.Deactivate();
        }
    }
    public void Reset()
    {
        SetLevel(currentLevelNumber);
    }
    public void Reset(int level) 
    {
        SetLevel(level); // Calls Reset();
    }
    public void Win()
    {
        Interactable.LockInteractables();
        StopAllCoroutinesGlobally();
        Debug.Log("you win!");
    }
    public void GameOver()
    {
        Interactable.LockInteractables();
        StopAllCoroutinesGlobally();
        Debug.Log("game over!");
    }

}