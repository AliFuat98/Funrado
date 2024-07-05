using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellFrog : Cell {
  private DrawLine drawLine;

  private void Start() {
    drawLine = GetComponent<DrawLine>();
  }

  public void DrawNextLine(Vector3 nextPoint) {
    drawLine.AddNewPointToDraw(nextPoint);
  }
}