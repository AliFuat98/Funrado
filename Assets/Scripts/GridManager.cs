using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
  public static GridManager Instance { get; private set; }

  [SerializeField] private LevelSO currentLevel;

  private Grid<GridObject> grid;

  [SerializeField] private int gridWidth;
  private int gridHeight;
  [SerializeField] private float cellSize = 2f;

  private void Awake() {
    Instance = this;
  }

  private void Start() {
    gridHeight = gridWidth;
    grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int z) => new GridObject());

    Setup();
  }

  public void Setup() {
    int stackIndex = 0;
    foreach (CellStackSO cellStack in currentLevel.cellStackSOList) {
      int depth = 0;
      foreach (CellSO cell in cellStack.CellSOList) {
        int x = Mathf.FloorToInt(stackIndex / gridWidth);
        float yOffset = depth * 0.1f;
        int z = stackIndex % gridWidth;
        Vector3 gridPosition = new Vector3(x, yOffset, z) * grid.GetCellSize() + .5f * grid.GetCellSize() * Vector3.one;
        Transform cellTransform = Instantiate(cell.prefab, gridPosition, Quaternion.identity);

        // store the transform in the GridObject
        grid.GetGridObject(x, z).Push(cellTransform);

        // assign cell variables
        cellTransform.GetComponent<Cell>().SetVar(x, z, depth, cell.cellColor);

        depth++;
      }

      stackIndex++;
    }

    // Show top level cell object (frog, grape, or direction)
    GridObject[,] gridArray = grid.GetGridArray();
    for (int x = 0; x < gridArray.GetLength(0); x++) {
      for (int z = 0; z < gridArray.GetLength(1); z++) {
        gridArray[x, z].Top().GetComponent<Cell>().ShowPlacedObject();
      }
    }
  }

  public void ClickCell(Cell cell) {
    var gridObject = grid.GetGridObject(cell.X, cell.Z);

    if (gridObject.IsTopCellFrog()) {
      Debug.Log("clicked frog");
      return;
    }

    Debug.Log("clicked other cells");
  }

  // ** transform is a container for cell **
  public class GridObject {
    private readonly Stack<Transform> stack = new();

    public void Push(Transform transform) {
      stack.Push(transform);
    }

    public Transform Pop() {
      var popedCell = stack.Pop();
      Destroy(popedCell.gameObject);

      if (stack.Count > 0) {
        var newTopCell = stack.Peek();
        newTopCell.GetComponent<Cell>().ShowPlacedObject();

        return popedCell;
      }
      return null;
    }

    public Transform Top() {
      if (stack.Count > 0) {
        return stack.Peek();
      }
      return null;
    }

    public bool IsTopCellFrog() {
      if (stack.Count > 0) {
        return stack.Peek().GetComponent<Cell>() is CellFrog;
      }
      return default;
    }

    public int StackCount() {
      return stack.Count;
    }
  }
}