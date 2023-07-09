using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : CustomSingleton<GridManager>
{
    [Header("Grid Settings")]
    [Space(5)]
    public int rowCount;
    public int columnCount; 
    public float elementPadding;
    private int cellSize = 1;

    [Header("References")]
    [Space(5)]
    public Transform cellContainer;
    public GridCell cellPrefab;

    public List<GridCell> cells;

    [Header("Grid Item Settings")]
    [Space(5)]
    public List<GridItemDataSO> GridItemDatas = new List<GridItemDataSO>();

   
    private void Start()
    {
        CanvasManager.instance.LevelIndexSelectedEvent += OnLevelSelected;
        LevelHandler.instance.LevelIsEndedEvent += OnLevelEnded;
    }

    private void OnLevelEnded(bool _)
    {
        ClearCurrentGridBase();
        
    }

    private void ClearCurrentGridBase()
    {
        cells.Clear();
        for (int i = 0; i < cellContainer.childCount; i++)
        {
            Destroy(cellContainer.GetChild(i).gameObject);
        }
        cellContainer.gameObject.SetActive(false);
    }
    private void OnLevelSelected(int index)
    {
        
        cellContainer.gameObject.SetActive(true);
        int levelIndex = index;
        JsonReader.Level levelData = JsonReader.instance.GetLevelData(levelIndex);
        columnCount = levelData.column;
        rowCount = levelData.row;

        cells = new List<GridCell>();

        for (int i = 0; i < columnCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                Vector3 worldPosition = new Vector3(i, j, 1) * (elementPadding + cellSize);

                GridCell cell = Instantiate(cellPrefab, worldPosition, Quaternion.identity, cellContainer);
                cell.id = new Vector2Int(i, j);
                cell.name = "Cell " + "(" + worldPosition.x + "," + worldPosition.y + ")";
                cell.InitializeGridCell(worldPosition);
                cells.Add(cell);
            }
        }

        FindAllNeighbors();
        
    }
    public void OnCellEmpty(GridCell currentCell)
    {
        if (currentCell.id.y >= cells[cells.Count - 1].id.y)
        {
            return;
        }

        var targetId = new Vector2Int(currentCell.id.x, currentCell.id.y + 1);
        var targetCell = cells.FirstOrDefault(cell => cell.id == targetId);

        if (targetCell.CheckIsOccupied())
        {
            GridItem targetItem = targetCell.GetGridItem();
            currentCell.SetNewItem(targetItem);
            targetCell.ClearGridItem();
        }

    }
    public IEnumerator FillEmptyCells()
    {
        yield return new WaitForSeconds(0.3f);
        if (LevelManager.instance.isLevelEnded) yield break;
        for (int i = 0; i <= cells[cells.Count-1].id.x; i++)
        {
            var column = cells.Where(cell => cell.id.x == i && !cell.CheckIsOccupied()).ToList();
            for (int j = 0; j < column.Count; j++)
            {
                Vector3 desiredPos = new Vector3(column[j].GetCoordinates().x, cells[cells.Count - 1].GetCoordinates().y + ((j+1)*(elementPadding + cellSize)),1);
                column[j].CreateGridItem(desiredPos, true);
            }
        }
        yield return new WaitForSeconds(0.1f);
    }
    [Button]
    public void ShuffleList()
    {
        var Items = cells.Select(cell => cell.GetGridItem()).ToList();
        Items.Shuffle();

        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].SetNewItem(Items[i], true);
        }
        FindAllNeighbors();
    }
    public void FindAllNeighbors()
    {
        foreach (GridCell cell in cells)
        {
            List<GridCell> neighbors = new List<GridCell>();

            int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };

            for (int i = 0; i < dx.Length; i++)
            {
                int newX = (int)cell.GetCoordinates().x + dx[i];
                int newY = (int)cell.GetCoordinates().y + dy[i];  

                var neighbor = cells.Where(neighborCell => neighborCell.id.x == newX && neighborCell.id.y == newY).ToList();
                if (neighbor.Count > 0)
                {
                    neighbors.AddRange(neighbor);
                }
            }

            cell.neighbors = neighbors;
        }
    }

   
}
