using System.Collections;
using UnityEngine;

public class Tongue : MonoBehaviour {
  public LayerMask grapeLayerMask;
  public LayerMask directionLayerMask;

  public Direction LookDirection { get; set; }
  public Vector2Int LookDirectionVector { get; set; }

  public bool IsMovingForward { get; private set; } = false;
  public bool IsMovingBackward { get; private set; } = false;

  private bool CanGoNextCell = false;

  private CellFrog frog;

  private Cell nextCandidateCell;

  private void Start() {
    frog = GetComponentInParent<CellFrog>();
  }

  public CellFrog GetFrog() {
    return frog;
  }

  public void StartMovingForward() {
    IsMovingForward = true;
    IsMovingBackward = false;
  }

  public void StartMovingBackward() {
    IsMovingForward = false;
    IsMovingBackward = true;
  }

  public void StopMoving() {
    // if stop moving function calling when the move is forward
    // then we can look at next cell in the trigger function
    if (IsMovingForward) {
      CanGoNextCell = true;
    }

    IsMovingForward = false;
    IsMovingBackward = false;

    if (frog.NextVisitedCell.IsCellBusy()) {
      frog.CancelCollection();
      CanGoNextCell = false;
      SoundManager.Instance.WrongMove();
    }
  }

  private void OnTriggerStay(Collider other) {
    if (!CanGoNextCell) {
      return;
    }
    CanGoNextCell = false;

    CheckGrape(other);
    CheckDirection(other);
  }

  public void CheckGrape(Collider other) {
    if ((grapeLayerMask.value & (1 << other.gameObject.layer)) != 0) {
      // touched a grape in here

      CellGrape cellGrape = other.GetComponentInParent<CellGrape>();

      // check => if the colors are the same || if the Cell is busy
      if (cellGrape.CellColor != frog.CellColor || cellGrape.IsCellBusy()) {
        frog.CancelCollection();
        return;
      }

      // check => if the next cell is out of box
      var grid = GridManager.Instance.grid;
      var gridObject = grid.GetGridObject(cellGrape.X, cellGrape.Z, LookDirectionVector);
      if (gridObject == null) {
        // FROG CAN EAT ALL GRAPES ON THE WAY
        cellGrape.SetBusy(true);
        StartCoroutine(StartEating(cellGrape));
        return;
      }

      // store this cell to check if it is busy or not
      var nextGridObject = grid.GetGridObject(cellGrape.X, cellGrape.Z, LookDirectionVector);
      nextCandidateCell = nextGridObject.TopCell();

      // there are other cell to continue the process
      // look at next cell
      frog.ContinueCollecting(cellGrape);
    }
  }

  private IEnumerator StartEating(Cell cell) {
    yield return new WaitForSeconds(1f);
    frog.StartEating(cell);
  }

  public void CheckDirection(Collider other) {
    if ((directionLayerMask.value & (1 << other.gameObject.layer)) != 0) {
      // touched a direction in here
      CellDirection cellDirection = other.GetComponentInParent<CellDirection>();

      // check => if the colors are the same || if the Cell is busy
      if (cellDirection.CellColor != frog.CellColor || cellDirection.IsCellBusy()) {
        SoundManager.Instance.WrongMove();
        frog.CancelCollection();
        return;
      }

      // change frog look direction.
      LookDirection = cellDirection.LookDirection;
      LookDirectionVector = cellDirection.LookDirectionVector;

      // check => if the next cell is out of box (this cannot be happen if we design the map)
      var grid = GridManager.Instance.grid;
      var gridObject = grid.GetGridObject(cellDirection.X, cellDirection.Z, LookDirectionVector);
      if (gridObject == null) {
        // the direction points to outside of the box cancel it
        SoundManager.Instance.WrongMove();
        frog.CancelCollection();
        return;
      }

      // store this cell to check if it is busy or not
      var nextGridObject = grid.GetGridObject(cellDirection.X, cellDirection.Z, LookDirectionVector);
      nextCandidateCell = nextGridObject.TopCell();

      frog.ContinueCollecting(cellDirection);
    }
  }
}