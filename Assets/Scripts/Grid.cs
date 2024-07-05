using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject> {
  private int width;
  private int height;
  private float cellSize;
  private Vector3 originPosition;
  private TGridObject[,] gridArray;

  public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) {
    this.width = width;
    this.height = height;
    this.cellSize = cellSize;
    this.originPosition = originPosition;

    gridArray = new TGridObject[width, height];

    // Create Grid
    for (int x = 0; x < gridArray.GetLength(0); x++) {
      for (int z = 0; z < gridArray.GetLength(1); z++) {
        gridArray[x, z] = createGridObject(this, x, z);
      }
    }
  }


  //public Vector3 GetWorldPosition(int x, int z) {
  //  return new Vector3(x, 0, z) * cellSize + originPosition;
  //}

  //public TGridObject GetGridObject(int x, int z) {
  //  if (x >= 0 && z >= 0 && x < width && z < height) {
  //    return gridArray[x, z];
  //  } else {
  //    return default(TGridObject);
  //  }
  //}

  // getters
  public int GetWidth() {
    return width;
  }

  public int GetHeight() {
    return height;
  }

  public float GetCellSize() {
    return cellSize;
  }

}