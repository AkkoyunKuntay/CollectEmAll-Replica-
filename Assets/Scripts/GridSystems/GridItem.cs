using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridItem : MonoBehaviour
{
    [SerializeField] GridItemDataSO itemData;
    public Image itemImage;
    public Image gridCellImage;

    private void Awake()
    {
        InitializeGridItem();
    }

    public GridItemDataSO GetItemData()
    {
        return itemData;
    }
    public void PlaySelectionAnimation()
    {
        transform.DOScale(Vector3.one, 0.5f).From(Vector3.one / 1.5f).SetLoops(-1, LoopType.Yoyo);
    }
    private void InitializeGridItem()
    {
        GetRandomData();
        itemImage.sprite = itemData.itemImage;
    }
    private void GetRandomData()
    {
        int randomIndex = Random.Range(0, GridManager.instance.GridItemDatas.Count);
        GridItemDataSO randomData = GridManager.instance.GridItemDatas[randomIndex];
        itemData = randomData;
    }
}


