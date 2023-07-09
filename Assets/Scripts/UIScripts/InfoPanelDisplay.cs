using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanelDisplay : CustomSingleton<InfoPanelDisplay>
{
    public TextMeshProUGUI moveCountTXT;

    private void Start()
    {
        LevelHandler.instance.LevelIsReadyEvent += OnLevelIsReady;
       

        
    }

    private void OnLevelIsReady()
    {
        SetDisplayingItems();
        UpdateMoveCount();
    }

    public void UpdateMoveCount()
    {
        moveCountTXT.text = LevelHandler.instance.moveCount.ToString();
    }
    private void SetDisplayingItems()
    {
        foreach (TargetItems item in LevelHandler.instance.targetItems)
        {
            if (item.CheckIsDisplayable())
            {
                item.displayObject.SetActive(true);
            }
            else
            {
                item.displayObject.SetActive(false);
            }
        }
        
    }
}
