using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : CustomSingleton<LevelManager>
{
    [SerializeField] List<LevelButtonController> levelButtons;
    public bool isLevelEnded;

    private void Start()
    {
        CanvasManager.instance.LevelIndexSelectedEvent += OnLevelSelected;
        LevelHandler.instance.LevelIsEndedEvent += OnLevelEnded;
        SetAvailableLevels();
    }

    private void OnLevelSelected(int obj)
    {
        isLevelEnded = false;
    }

    private void OnLevelEnded(bool _)
    {
        SetAvailableLevels();
        isLevelEnded = true;
    }

    public void SetAvailableLevels()
    {
        foreach (LevelButtonController level in levelButtons) 
        {
            if (level.IsLevelCompleted())
            {

                LevelButtonController nextLevel = GetLevelByIndex(level.level + 1);

                if (nextLevel != null)
                {
                    nextLevel.SetIsPlayable(true);
                }
                else
                {
                    return;
                }
            }
        }
    }
    public LevelButtonController GetLevelByIndex(int levelIndex)
    {   
        if(levelIndex <= levelButtons.Count-1) return levelButtons[levelIndex];
        else return null;

    }
    
}
