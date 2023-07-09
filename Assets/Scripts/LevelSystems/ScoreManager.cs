using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : CustomSingleton<ScoreManager>
{
    public int score;
    public LevelButtonController targetLevel;

    private void Start()
    {
        LevelHandler.instance.LevelIsEndedEvent += OnLevelEnded;
        SelectionHandler.instance.SelectionsAreDestroyedEvent += OnSelectionsDestroyed;
    }

    private void OnLevelEnded(bool _)
    {
       score = 0;
    }

    private void OnSelectionsDestroyed(ItemType _, int selectionCount)
    {
        SetScore(selectionCount);
        int targetLevel = LevelHandler.instance.levelIndex;
        LevelButtonController level = LevelManager.instance.GetLevelByIndex(targetLevel);
        if (level.highestScore > score) { return; }

        PlayerPrefs.SetInt(level.name, score);
    }
    public void SetScore(int validScore)
    {
        score += validScore;
    }
    public int GetScore(int levelIndex)
    {
        string saveName = "Level" + levelIndex;
        return PlayerPrefs.GetInt(saveName, 0);
    }

    
}
