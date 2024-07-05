using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CellSO : ScriptableObject {
  public string nameString;
  public Transform prefab;
  public CellColor cellColor;
}

public enum CellColor {
  Blue,
  Green,
  Purple,
  Red,
  Yellow,
}