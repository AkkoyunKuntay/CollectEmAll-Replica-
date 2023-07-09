using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType { None,Banana,Apple,DragonFruit,BlueBerry,Drop }

[CreateAssetMenu(fileName = "GridItemData",menuName = "NewItemData")]
public class GridItemDataSO : ScriptableObject
{
    public ItemType itemType;
    public Sprite itemImage;
    
}
