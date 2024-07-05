using UnityEngine;

public class CellFrog : Cell {
  private DrawLine drawLine;

  private void Start() {
    drawLine = GetComponent<DrawLine>();
  }

  private void DrawNextLine(Vector3 nextPoint) {
    drawLine.AddNewPointToDraw(nextPoint);
  }

  public override void StartEating() {
    var grid = GridManager.Instance.grid;

    var gridObject = grid.GetGridObject(X, Z);

    Cell topCell = gridObject.TopCell();

    if (topCell is CellFrog) {
      CellFrog frog = gridObject.TopCell() as CellFrog;

      var nextGridObject = grid.GetGridObject(X, Z, LookDirectionVector);

      float offset = .2f;
      frog.DrawNextLine(nextGridObject.TopCellPosition() + new Vector3(0, offset, 0));

      return;
    }

    Debug.Log("clicked other cells");
  }
}