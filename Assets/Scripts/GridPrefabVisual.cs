using UnityEngine;

public class GridPrefabVisual : MonoBehaviour {
  [SerializeField] private LevelSO currentLevel;

  private Transform[,] visualNodeArray;
  private Grid<GridObject> grid;

  private void Start() {
    int gridWidth = 2;
    int gridHeight = 2;
    float cellSize = 2f;

    grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z));

    Setup(grid);
  }

  public void Setup(Grid<GridObject> grid) {
    this.grid = grid;
    visualNodeArray = new Transform[grid.GetWidth(), grid.GetHeight()];

    int stackIndex = 0;
    foreach (CellStackSO cellStack in currentLevel.cellStackSOList) {
      int depth = 0;
      foreach (CellSO cell in cellStack.CellSOList) {
        int x = Mathf.FloorToInt(stackIndex / 2);
        float yOffset = depth * 0.1f;
        int z = stackIndex % 2;
        Vector3 gridPosition = new Vector3(x, yOffset, z) * grid.GetCellSize() + .5f * grid.GetCellSize() * Vector3.one;
        Transform cellTransform = Instantiate(cell.prefab, gridPosition, Quaternion.identity);
        cellTransform.GetComponent<Cell>().SetVar(x, z, depth, cell.cellColor);
        depth++;
      }

      stackIndex++;
    }
  }

  public class GridObject {
    private Grid<GridObject> grid;
    private int x;
    private int z;
    private int value;
    private Vector3 position;

    public GridObject(Grid<GridObject> grid, int x, int z) {
      this.grid = grid;
      this.x = x;
      this.z = z;
      position = new Vector3(x, 0, z) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f;
    }

    public void SetValue(int value) {
      this.value = value;
      //grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString() {
      return x + "," + z + "\n" + value.ToString();
    }
  }
}