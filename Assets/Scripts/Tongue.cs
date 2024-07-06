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
      CellGrape cellGrape = other.GetComponentInParent<CellGrape>();

      // check => colors are the same
      if (cellGrape.Color != frog.Color) {
        // play wrong grape animation
        // reset the tongue verticies
        // release the busy cells
        Debug.Log("color is not the same");
        return;
      }

      // check ==> Cell is busy

      if (cellGrape.IsCellBusy()) {
        // play wrong grape animation
        // reset the tongue verticies
        // release the busy cells

        Debug.Log("cell busy");
        return;
      }

      // check => next cell is out of box
      var grid = GridManager.Instance.grid;
      var gridObject = grid.GetGridObject(cellGrape.X, cellGrape.Z, frog.LookDirectionVector);
      if (gridObject == null) {
        // FROG CAN EAT ALL GRAPES ON THE WAY
        Debug.Log("FROG CAN EAT ALL GRAPES ON THE WAY");
        return;
      }
      // there are other grape cell continue the process
      // look at next cell
      Debug.Log("look at next cell");

      // add cell to visited cell stack
      frog.ContinueEating(cellGrape);
    }
  }
}