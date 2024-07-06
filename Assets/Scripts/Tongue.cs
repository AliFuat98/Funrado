using Unity.VisualScripting;
using UnityEngine;

public class Tongue : MonoBehaviour {
  public LayerMask grapeLayerMask;

  private bool isMovingForward = false;
  private bool isMovingBackward = false;

  private CellFrog frog;

  private void Start() {
    frog = GetComponentInParent<CellFrog>();
  }

  public bool isMoving() {
    return isMovingBackward || isMovingForward;
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
    isMovingForward = false;
    isMovingBackward = false;
  }

  void OnCollisionEnter(Collision collision) {
    Debug.Log("enter");
    if ((grapeLayerMask.value & (1 << collision.gameObject.layer)) != 0)
      Debug.Log(collision.transform.name);
  }

  private void OnTriggerEnter(Collider other) {
    if (!isMoving()) {
      return;
    }
    Debug.Log("enter triggger");
    if ((grapeLayerMask.value & (1 << other.gameObject.layer)) != 0) {
      CellGrape cellGrape = other.GetComponentInParent<CellGrape>();

      // check => colors are the same
      if (cellGrape.Color != frog.Color) {
        // play wrong grape animation
        // reset the tongue verticies
        // release the busy cells
        return;
      }

      // check ==> Cell is busy

      if (cellGrape.IsCellBusy()) {
        // play wrong grape animation
        // reset the tongue verticies
        // release the busy cells
        return;
      }

      // check => next cell is out of box
      var grid = GridManager.Instance.grid;
      var gridObject = grid.GetGridObject(cellGrape.X, cellGrape.Z, frog.LookDirectionVector);
      if (gridObject == null) {
        // FROG CAN EAT ALL GRAPES ON THE WAY
        return;
      }
      // there are other grape cell continue the process
      // look at next cell
    }
  }
}