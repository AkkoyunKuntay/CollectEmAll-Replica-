using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionAnimator : MonoBehaviour
{
    public float scaleFactor;
    [SerializeField] bool isSelected;
    [SerializeField] GridItem gridItem;

    Tween scaleLoopTween;
    private void Awake()
    {
        gridItem = GetComponent<GridItem>();
    }
    private void Start()
    {

        SelectionHandler.instance.GridCellSelectedEvent += OnGridCellSelected;
        SelectionHandler.instance.SelectionEndedEvent += OnSelectionEnded;
    }
    private void ScaleLoopComplete()
    {
        Image cellImage = gridItem.gridCellImage;
        Image itemImage = gridItem.itemImage;
        cellImage.transform.DOScale(Vector3.one, 0.1f);
        itemImage.transform.DOScale(Vector3.one, 0.1f);

    }
    private void OnSelectionEnded()
    {
        if (!isSelected) return;

        isSelected = false;  
        
        scaleLoopTween.OnComplete(null).onComplete = ScaleLoopComplete;
        scaleLoopTween.SetAutoKill(true);
    }

    private void OnGridCellSelected(GridItem selectedItem)
    {
        if (selectedItem == gridItem)
        {
            isSelected = true;

            Image itemImage = selectedItem.itemImage;
            Image cellImage = selectedItem.gridCellImage;
            cellImage.transform.DOScale((transform.localScale + (Vector3.one * scaleFactor)), 0.25f);
            scaleLoopTween = itemImage.transform.DOScale((transform.localScale + (Vector3.one * scaleFactor)), 0.3f).From(transform.localScale).SetLoops(2,LoopType.Yoyo).SetAutoKill(false);
            scaleLoopTween.OnComplete(() => scaleLoopTween.Restart());
        }
    }


}
