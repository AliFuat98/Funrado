using UnityEngine;

public class Cell : MonoBehaviour {
  public int x;
  public int z;
  public int k;
  public CellColor color;

  public void SetVar(int x, int z, int k, CellColor color) {
    this.x = x; this.z = z; this.k = k; this.color = color;
  }
}