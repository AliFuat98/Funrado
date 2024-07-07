using System.Collections.Generic;
using UnityEngine;

public class CellFrog : Cell {
  private DrawLine drawLine;

  private readonly Stack<Cell> visitedCellStack = new();

  private Tongue tongue;

  private void Start() {
    drawLine = GetComponent<DrawLine>();
    tongue = GetComponentInChildren<Tongue>();
    ResetTongueDirection();
  }

  private void DrawNextLine(Vector3 nextPoint) {
    drawLine.AddNewPointToDraw(nextPoint);
  }

  public override void StartCollecting() {
    if (IsCellBusy()) {
      return;
    }

    var grid = GridManager.Instance.grid;

    var gridObject = grid.GetGridObject(X, Z);

    Cell topCell = gridObject.TopCell();

    if (topCell is CellFrog) {
      // allocate the cell
      SetBusy(true);

      CellFrog frog = gridObject.TopCell() as CellFrog;
      visitedCellStack.Push(frog);

      var nextGridObject = grid.GetGridObject(X, Z, tongue.LookDirectionVector);

      float offset = .2f;
      frog.DrawNextLine(nextGridObject.TopCellPosition() + new Vector3(0, offset, 0));

      return;
    }

    Debug.Log("clicked other cells");
  }

  public void ContinueCollecting(Cell lastCell) {
    visitedCellStack.Push(lastCell);
    lastCell.SetBusy(true);
    if (lastCell is CellGrape) {
      lastCell.GetComponentInChildren<GrapeMovement>().SetFrog(this);
      SoundManager.Instance.CollectGrape(0);
    }

    var grid = GridManager.Instance.grid;

    var nextGridObject = grid.GetGridObject(lastCell.X, lastCell.Z, tongue.LookDirectionVector);
    float offset = .2f;
    DrawNextLine(nextGridObject.TopCellPosition() + new Vector3(0, offset, 0));
  }

  // if the player's move is correct then this function will be called
  public void StartEating(Cell lastCell) {
    visitedCellStack.Push(lastCell);
    lastCell.SetBusy(true);
    if (lastCell is CellGrape) {
      lastCell.GetComponentInChildren<GrapeMovement>().SetFrog(this);
    }

    // get back the tongue
    drawLine.UndoAllPoints();

    // move a grape
    MoveNextGrape();
  }

  public void MoveNextGrape() {
    var cell = visitedCellStack.Pop();
    if (cell is CellGrape) {
      cell.GetComponentInChildren<GrapeMovement>().MoveToFrog(visitedCellStack.ToArray());
      cell.GetPlacedObject().transform.SetParent(null);
    }
  }

  public void CancelCollection() {
    // relase visited cells
    foreach (var cell in visitedCellStack) {
      cell.SetBusy(false);
    }

    // release frog
    SetBusy(false);

    // reset tongue direction
    ResetTongueDirection();

    // delete the tongue line
    drawLine.ClearTheLine();
  }

  private void ResetTongueDirection() {
    tongue.LookDirection = LookDirection;
    tongue.LookDirectionVector = LookDirectionVector;
  }
}