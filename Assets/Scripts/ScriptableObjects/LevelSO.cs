using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSO : ScriptableObject {
  public List<CellStackSO> cellStackSOList = new();
  public int levelMoveCount;
  public int width;
  public int gameLevel;
}