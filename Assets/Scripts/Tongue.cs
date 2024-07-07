using UnityEngine;

public class Tongue : MonoBehaviour {
  public LayerMask grapeLayerMask;

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

    if ((grapeLayerMask.value & (1 << other.gameObject.layer)) != 0) {
      // touched a grape in here

      CellGrape cellGrape = other.GetComponentInParent<CellGrape>();

      // check => colors are the same
      // check ==> Cell is busy
      if (cellGrape.CellColor != frog.CellColor || cellGrape.IsCellBusy()) {
        // play wrong grape animation
        // reset the tongue verticies
        // release the busy cells

        Debug.Log("color is not the same");
        frog.CancelEating();
        return;
      }

      // check => next cell is out of box

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
}