using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFrog : Cell {
  private DrawLine drawLine;

  private readonly Stack<Cell> visitedCellStack = new();

  private Tongue tongue;

  public Cell NextVisitedCell { get; private set; }

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
      topCell.SetBusy(true);
      GameManager.Instance.DecreaseMoveCount();
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

    if (nextGridObject == null) {
      // then frog is looking out of box
      CancelCollection();
      return;
    }

    // store this to check if the next cell is busy in the tongue scripts
    NextVisitedCell = nextGridObject.TopCell();

    DrawNextLine(nextGridObject.TopCellPosition() + new Vector3(0, offset, 0));
  }

  // if the player's move is correct then this function will be called
  public void StartEating(Cell lastCell) {
    SoundManager.Instance.GetBackTongue();


    visitedCellStack.Push(lastCell);
    lastCell.SetBusy(true);

    // assign lastGrape
    AssignLastGrape();

    // get back the tongue
    drawLine.UndoAllPoints();

    // destroy the cells
    StartCoroutine(GetNextCells());

    // move a grape
    MoveNextGrape();
  }

  private void AssignLastGrape() {
    CellGrape lastGrape = null;
    foreach (var cell in visitedCellStack.ToArray()) {
      if (cell is CellGrape) {
        lastGrape = cell as CellGrape;
      }
    }

    lastGrape.GetComponentInChildren<Grape>().lastGrape = true;
  }

  private IEnumerator GetNextCells() {
    float duration = GameManager.Instance.GetDuration();

    var visitedList = visitedCellStack.ToArray();
    for (int i = 0; i <= visitedList.Length - 1; i++) {
      yield return new WaitForSeconds(duration);
      GridManager.Instance.GetNextCell(visitedList[i].X, visitedList[i].Z);
    }
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
    NextVisitedCell = null;

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