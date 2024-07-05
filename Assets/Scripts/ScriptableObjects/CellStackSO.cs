using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CellStackSO : ScriptableObject {
  public List<CellSO> CellSOList = new();
}