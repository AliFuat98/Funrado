using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CellSO : ScriptableObject {
  public string nameString;
  public Transform prefab;
  public CellColor cellColor;
  public Direction direction;
}

public enum CellColor {
  Blue,
  Green,
  Purple,
  Red,
  Yellow,
}

public enum Direction {
  Up,
  Down,
  Left,
  Right,
}