using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionHandler : CustomSingleton<SelectionHandler>
{
    private GridItemDataSO lastItemData;

    [Header("Debug")]
    [SerializeField] Vector2 lastSelectedCoords;
    [SerializeField] GridItem lastSelectedGridItem;
    [SerializeField] ItemType selectedType;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] List<GridItem> selectedItems = new List<GridItem>();


    public event System.Action<GridItem> GridCellSelectedEvent;
    public event System.Action SelectionEndedEvent;
    public event System.Action<bool> SelectionValidEvent;
    public event System.Action<ItemType, int> SelectionsAreDestroyedEvent;

    private void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();

    }

    public void Update()
    {
        if(Input.GetMouseButton(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent(out GridCell gridCell))
                {
                    GridItem potentialSelectionItem = gridCell.GetGridItem();
                    if (potentialSelectionItem == null)
                    {
                        return;
                    }

                    if (lastSelectedGridItem == null)
                    {
                        SetItemAsSelected(potentialSelectionItem);
                        lastSelectedCoords = gridCell.GetCoordinates();
                    }
                    else
                    {
                        Vector2 potentialSelectionCoordinates = gridCell.GetCoordinates();
                        float xDiff = Mathf.Abs(lastSelectedCoords.x - potentialSelectionCoordinates.x);
                        float yDiff = Mathf.Abs(lastSelectedCoords.y - potentialSelectionCoordinates.y);

                        if (xDiff > 1.75f) return;
                        if (yDiff > 1.75f) return;

                        if (potentialSelectionItem.GetItemData().itemType == lastItemData.itemType)
                        {
                            if (!selectedItems.Contains(potentialSelectionItem))
                            {
                                SetItemAsSelected(potentialSelectionItem);
                                lastSelectedCoords = potentialSelectionCoordinates;
                                lineRenderer.positionCount = selectedItems.Count;

                                for (int i = 0; i < selectedItems.Count; i++)
                                {
                                    lineRenderer.SetPosition(i, selectedItems[i].GetComponentInParent<GridCell>().GetCoordinates());
                                }
                            }
                        }
                    }

                   
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {

            lineRenderer.positionCount = 0;

            if (selectedItems.Count < 3)
            {
                SelectionValidEvent?.Invoke(false);
                selectedItems.Clear();

                lastSelectedGridItem = null;
                selectedType = ItemType.None;
                lastSelectedCoords = Vector2.zero;

                SelectionEndedEvent?.Invoke();

                return;
            }
            else
            {
                List<GridCell> cells = new List<GridCell>();
                foreach (var item in selectedItems)
                {
                    GridCell selectedCell = item.GetComponentInParent<GridCell>();
                    cells.Add(selectedCell);
                    selectedCell.DestoyGridItem();
                }
                selectedItems.Clear();
                cells = cells.OrderByDescending(cell => cell.id.y).ToList();

                foreach (var cell in cells)
                {
                    GridManager.instance.OnCellEmpty(cell);
                }
                StartCoroutine(GridManager.instance.FillEmptyCells());
                SelectionValidEvent?.Invoke(true);
                SelectionsAreDestroyedEvent?.Invoke(selectedType, cells.Count);
            }

            lastSelectedGridItem = null;
            selectedType = ItemType.None;
            lastSelectedCoords = Vector2.zero;

            SelectionEndedEvent?.Invoke();
        }
    }

    private void SetItemAsSelected(GridItem gridItem)
    {
        GridCellSelectedEvent?.Invoke(gridItem);

        lastSelectedGridItem = gridItem;
        lastItemData = lastSelectedGridItem.GetItemData();
        selectedType = lastItemData.itemType;
        selectedItems.Add(lastSelectedGridItem);
    }
}
