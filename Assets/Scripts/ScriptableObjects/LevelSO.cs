using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSO : ScriptableObject {
  public List<CellStackSO> cellStackSOList = new();
}