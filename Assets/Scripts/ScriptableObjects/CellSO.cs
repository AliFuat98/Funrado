using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CellSO : ScriptableObject {
  public string nameString;
  public Transform prefab;
  public int x;
  public int z;
  public int k;
}