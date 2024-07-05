using UnityEngine;

public class GridPrefabVisual : MonoBehaviour {

  [SerializeField] private Transform pfGridPrefabVisualNode = null;

  private Transform[,] visualNodeArray;
  private Grid<GridObject> grid;

  private void Start() {
    int gridWidth = 6;
    int gridHeight = 6;
    float cellSize = 1f;

    grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z));

    Setup(grid);
  }

  public void Setup(Grid<GridObject> grid) {
    this.grid = grid;
    visualNodeArray = new Transform[grid.GetWidth(), grid.GetHeight()];

    for (int x = 0; x < grid.GetWidth(); x++) {
      for (int z = 0; z < grid.GetHeight(); z++) {
        Vector3 gridPosition = new Vector3(x,0, z) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f;
        Transform visualNode = Instantiate(pfGridPrefabVisualNode, gridPosition, Quaternion.identity);
        visualNodeArray[x, z] = visualNode;
      }
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

