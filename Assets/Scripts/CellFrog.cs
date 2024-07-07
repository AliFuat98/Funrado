using System.Collections.Generic;
using UnityEngine;

public class CellFrog : Cell {
  private DrawLine drawLine;

  private readonly Stack<Cell> visitedCellStack = new();

  private Tongue tongue;
  private Animator animator;

  private void Start() {
    drawLine = GetComponent<DrawLine>();
    tongue = GetComponentInChildren<Tongue>();
    animator = GetComponent<Animator>();
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
      animator.SetTrigger("Clicked");
      return;
    }

    Debug.Log("clicked wrong cell");
  }

  public void ContinueCollecting(Cell lastCell) {
    var grid = GridManager.Instance.grid;

    if (lastCell == null) {
      // getting from trigger animation event
      var gridObject = grid.GetGridObject(X, Z);
      lastCell = gridObject.TopCell();
      animator.SetBool("IsCollecting", true);
    }

    visitedCellStack.Push(lastCell);
    lastCell.SetBusy(true);

    var nextGridObject = grid.GetGridObject(lastCell.X, lastCell.Z, tongue.LookDirectionVector);
    float offset = .2f;
    DrawNextLine(nextGridObject.TopCellPosition() + new Vector3(0, offset, 0));
  }

  // if the player's move is correct then this function will be called
  public void StartEating(Cell lastCell) {
    SoundManager.Instance.GetBackTongue();
    visitedCellStack.Push(lastCell);
    lastCell.SetBusy(true);

    // get back the tongue
    drawLine.UndoAllPoints();

    // move a grape
    MoveNextGrape();
  }

  public void MoveNextGrape() {
    var cell = visitedCellStack.Pop();
    if (cell is CellGrape) {
      cell.GetComponentInChildren<Grape>().MoveToFrog(visitedCellStack.ToArray());
    }
  }

  public void CancelCollection() {
    // relase visited cells
    foreach (var cell in visitedCellStack) {
      cell.SetBusy(false);
    }
    visitedCellStack.Clear();

    // release frog
    SetBusy(false);

    // reset tongue direction
    ResetTongueDirection();

    // delete the tongue line
    drawLine.ClearTheLine();

    // reset animation
    animator.SetBool("IsCollecting", false);
  }

  private void ResetTongueDirection() {
    tongue.LookDirection = LookDirection;
    tongue.LookDirectionVector = LookDirectionVector;
  }
}