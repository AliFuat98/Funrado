using System.Collections;
using UnityEngine;

public class GrapeMovement : MonoBehaviour {
  public LayerMask grapeLayerMask;
  private Cell[] cells;
  private float duration = 1.0f;
  private bool IsMoving = false;
  private CellFrog frog = null;

  public void SetFrog(CellFrog frog) {
    this.frog = frog;
  }

  public void MoveToFrog(Cell[] cells) {
    this.cells = cells;

    StartCoroutine(MoveAlongPoints());
  }

  private IEnumerator MoveAlongPoints() {
    IsMoving = true;
    for (int i = 0; i <= cells.Length - 1; i++) {
      Vector3 startPoint = transform.position;
      float offset = 0.2f;
      Vector3 endPoint = cells[i].transform.position + new Vector3(0, offset, 0);
      float elapsedTime = 0;

      while (elapsedTime < duration) {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / duration;
        transform.position = Vector3.Lerp(startPoint, endPoint, t);
        yield return null;
      }

      transform.position = endPoint; // Ensure the grape reaches the exact position
    }
  }

  private void OnTriggerEnter(Collider other) {
    if (IsMoving) {
      return;
    }
    if ((grapeLayerMask.value & (1 << other.gameObject.layer)) != 0) {
      var otherGrape = other.GetComponent<GrapeMovement>();
      otherGrape.frog = frog;
      frog.MoveNextGrape();
    }
  }
}