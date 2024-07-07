using UnityEngine;

public class Tongue : MonoBehaviour {
  public LayerMask grapeLayerMask;
  public LayerMask directionLayerMask;

  private bool isMovingForward = false;
  private bool isMovingBackward = false;

  private bool CanGoNextCell = false;

  private CellFrog frog;

  private void Start() {
    frog = GetComponentInParent<CellFrog>();
  }

  public void StartMovingForward() {
    isMovingForward = true;
    isMovingBackward = false;
  }

  public void StartMovingBackward() {
    isMovingForward = false;
    isMovingBackward = true;
  }

  public void StopMoving() {
    // if stop moving function calling when the move is forward
    // then we can look at next cell in the trigger function
    if (isMovingForward) {
      CanGoNextCell = true;
    }

    isMovingForward = false;
    isMovingBackward = false;
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
        frog.CancelEating();
        return;
      }

      // check => if the next cell is out of box
      var grid = GridManager.Instance.grid;
      var gridObject = grid.GetGridObject(cellGrape.X, cellGrape.Z, frog.LookDirectionVector);
      if (gridObject == null) {
        // FROG CAN EAT ALL GRAPES ON THE WAY
        frog.FinishEating(cellGrape);
        return;
      }

      // there are other cell to continue the process
      // look at next cell
      frog.ContinueEating(cellGrape);
    }
  }

  public void CheckDirection(Collider other) {
    if ((directionLayerMask.value & (1 << other.gameObject.layer)) != 0) {
      Debug.Log("collide with a direction");
    }
  }
}