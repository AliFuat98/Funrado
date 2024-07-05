using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
  public static GridManager Instance { get; private set; }

  [SerializeField] private LevelSO currentLevel;

  public Grid<GridObject> grid { get; private set; }

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
        float yOffset = depth * 0.05f;
        int z = stackIndex % gridWidth;
        Vector3 gridPosition = new Vector3(x, yOffset, z) * grid.GetCellSize() + .5f * grid.GetCellSize() * Vector3.one;
        Transform cellTransform = Instantiate(cell.prefab, gridPosition, Quaternion.identity);

        // store the transform in the GridObject
        grid.GetGridObject(x, z).Push(cellTransform);

        // assign cell variables
        cellTransform.GetComponent<Cell>().SetVar(x, z, depth, cell.cellColor, cell.direction);

        depth++;
      }

      stackIndex++;
    }

    // Show top level cell object (frog, grape, or direction)
    GridObject[,] gridArray = grid.GetGridArray();
    for (int x = 0; x < gridArray.GetLength(0); x++) {
      for (int z = 0; z < gridArray.GetLength(1); z++) {
        gridArray[x, z].TopCell().ShowPlacedObject();
      }
    }
  }

  // ** transform is a container for cell **
  public class GridObject {
    private readonly Stack<Transform> stack = new();

    public void Push(Transform transform) {
      stack.Push(transform);
    }

    public Transform Pop() {
      var popedCellTransform = stack.Pop();
      Destroy(popedCellTransform.gameObject);

      if (stack.Count > 0) {
        var newTopCell = stack.Peek();
        newTopCell.GetComponent<Cell>().ShowPlacedObject();

        return popedCellTransform;
      }
      return null;
    }

    public Cell TopCell() {
      if (stack.Count > 0) {
        return stack.Peek().GetComponent<Cell>();
      }
      return null;
    }

    public Vector3 TopCellPosition() {
      if (stack.Count > 0) {
        return stack.Peek().position;
      }
      return default;
    }

    public int StackCount() {
      return stack.Count;
    }
  }
}