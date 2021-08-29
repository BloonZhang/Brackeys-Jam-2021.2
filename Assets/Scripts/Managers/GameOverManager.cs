using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    //////// Singleton shenanigans ////////
    private static GameOverManager _instance;
    public static GameOverManager Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    // objects for game over. If any are triggered, it's over
    public List<ForStageList> gameOverObjects;

    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}
    }

    // public methods
    public void Triggered(string tag)
    {
        List<Interactable> gameOverObjectsForStage = new List<Interactable>();
        try { gameOverObjectsForStage = gameOverObjects[LevelManager.Instance.currentLevelNumber - 1]; }
        catch (Exception e) { Debug.Log("Error in GameOverManager.Triggered()"); }
        // Debug.Log(tag);
        foreach(Interactable gameOverObject in gameOverObjectsForStage)
        {
            if (gameOverObject.tag == tag) { LevelManager.Instance.GameOver(); }
        }
    }
}
