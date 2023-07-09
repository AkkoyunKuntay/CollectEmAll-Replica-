using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [Header("References")]
    public GridItem gridItemPrefab;
    public ParticleSystem gridExpo;

    [Header("Debug")]
    [SerializeField] Vector3 coordinateVec;
    public Vector2Int id;
    [SerializeField] bool isOccupied;
    [SerializeField] GridItem gridItem;
    public List<GridCell> neighbors = new List<GridCell>();

    private void Start()
    {
        CreateGridItem(coordinateVec);
    }

    public void InitializeGridCell(Vector3 positionVector)
    {
        coordinateVec = positionVector;

    }
    public Vector2 GetCoordinates()
    {
        return coordinateVec;
    }
    public GridItem GetGridItem()
    {
        return gridItem;
    }
    public void SetNewItem(GridItem newItem,bool ignoreOccupation = false)
    {
        if(!ignoreOccupation && CheckIsOccupied()) 
        {
            Debug.Log("Returning");
            return; 
        }

        isOccupied = true;
        gridItem = newItem;
        gridItem.transform.SetParent(transform);
        gridItem.transform.DOLocalMove(Vector3.zero, 0.3f).SetEase(Ease.Linear);

    }
    public void ClearGridItem()
    {
        gridItem = null;
        isOccupied = false;
        GridManager.instance.OnCellEmpty(this);
    }
    public bool CheckIsOccupied()
    {
        isOccupied = gridItem != null ? true : false;
        return isOccupied;
    }
    public GridItem CreateGridItem(Vector3 pos, bool isAnimating = false)
    {
        if (CheckIsOccupied()) return gridItem;

        gridItem = Instantiate(gridItemPrefab, pos, Quaternion.identity,transform);
        isOccupied =true;
        if (isAnimating)
        {
            gridItem.transform.DOLocalMoveY(0, 0.3f);
        }
        return gridItem;

    }
    public void DestoyGridItem()
    {
        Instantiate(gridExpo, coordinateVec, Quaternion.identity);
        Destroy(gridItem.gameObject);
        
        gridItem = null;
        isOccupied= false;
    }
    public void HasSameNeighbor()
    {
        foreach (var neighborCell in neighbors)
        {
           ItemType neighborType = neighborCell.GetGridItem().GetItemData().itemType;
            if(gridItem.GetItemData().itemType == neighborType)
            {
                GridCell newNode = neighborCell;
                foreach (var neighbor in newNode.neighbors)
                {
                    if (neighbor.GetGridItem().GetItemData().itemType == neighborType)
                    {
                        Debug.Log("There Is a different way!");
                        break;
                    }
                    break;
                }
                break;
            }
            Debug.Log("NO WAY! Need To Shuffle");
        }
       
    }
}
