using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static JsonReader;


public class LevelHandler : CustomSingleton<LevelHandler>
{
    public int moveCount;
    public List<TargetItems> targetItems;

    public event System.Action OutOfMovesEvent;
    public event System.Action LevelIsReadyEvent;
    public event System.Action<bool> LevelIsEndedEvent;

    public bool isLevelReady;
    public int levelIndex;

    private IEnumerator Start()
    {
        CanvasManager.instance.LevelIndexSelectedEvent += OnLevelSelected;
        SelectionHandler.instance.SelectionValidEvent += OnSelectionValid;
        SelectionHandler.instance.SelectionsAreDestroyedEvent += OnSelectionsAreDestroyed;

        yield return new WaitUntil(() =>
        {
            return isLevelReady;
        });
        ReadTargetItemsDataFromJson();
        InitializeTargetItems();
        LevelIsReadyEvent?.Invoke();
    }

    private void ReadTargetItemsDataFromJson()
    {
        JsonReader.Level levelData = JsonReader.instance.GetLevelData(levelIndex);
        moveCount = levelData.total_move;
        for (int i = 0; i < levelData.target_objectives.Length; i++)
        {
            TargetObjective targetObjective = levelData.target_objectives[i];
            for (int j = 0; j < targetItems.Count; j++)
            {
                if (targetObjective.name == targetItems[j].name)
                {
                    targetItems[j].targetAmount = targetObjective.count;
                }
            }
        }
    }

    private void OnLevelSelected(int index)
    {
        levelIndex = index;
        ReadTargetItemsDataFromJson();
        InitializeTargetItems();

        isLevelReady = true; 
        LevelIsReadyEvent?.Invoke();
        
    }
    private void InitializeTargetItems()
    {
        foreach (var item in targetItems)
        {
            item.CheckIsDisplayable();
            item.UpdateDisplayText();
        }
    }
    private void OnSelectionsAreDestroyed(ItemType targetType, int targetAmount)
    {
        UpdateTargetItemCounts(targetType, targetAmount);
    }
    public bool HasMove()
    {
        return moveCount > 0;
    }
    private void OnSelectionValid(bool isValid)
    {
        if(isValid) 
        { 
            DecreaseMoveCount();
            InfoPanelDisplay.instance.UpdateMoveCount();
            
        }
    }
    private void DecreaseMoveCount()
    {
        if(HasMove()) moveCount--;
    }
    private bool CheckLevelCompleted()
    {
        foreach (var item in targetItems)
        {
            if(!item.CheckIsCompleted()) return false;
        }
        return true;
    }
    private void UpdateTargetItemCounts(ItemType targetType, int amount)
    {
        foreach (var item in targetItems)
        {
            if(item.type == targetType)
            {
                if (!item.CheckIsCompleted())
                {
                    item.targetAmount -= amount;
                    item.UpdateDisplayText();
                    item.CheckIsCompleted();
                }
            }
        }
        if (!CheckLevelCompleted())
        {
            if (!HasMove())
            {
                OutOfMovesEvent?.Invoke();
                Debug.Log("LEVEL FAILED!!!");
                StartCoroutine(CanvasManager.instance.LevelEndRoutine(false));
                LevelIsEndedEvent?.Invoke(false);
            }
        }
        else
        {
            Debug.Log("LEVEL COMPLETED!!!");
            LevelManager.instance.GetLevelByIndex(levelIndex).SetLevelAsCompleted();
            StartCoroutine(CanvasManager.instance.LevelEndRoutine(true));
            LevelIsEndedEvent?.Invoke(true);
        }

    }

    
}



[System.Serializable]
public class TargetItems
{
    public string name;
    public ItemType type;
    public int targetAmount;
    public GameObject displayObject;
    [SerializeField] bool displayable;
    [SerializeField] bool isCompleted;

    public TextMeshProUGUI countText;

    public bool CheckIsDisplayable()
    {
        displayable = targetAmount > 0 ? true : false;
        if (!displayable) isCompleted = true;
        return displayable;
    }
    public bool CheckIsCompleted()
    {
        isCompleted = targetAmount <= 0 ? true: false;
        return isCompleted;
    }
    public void UpdateDisplayText()
    {
        int displayCount = targetAmount;

        if (targetAmount < 0) displayCount = 0;
        countText.text = displayCount.ToString();
    }
}
