using UnityEngine;

public class Cell : MonoBehaviour {
  public int X { get; private set; }
  public int Z { get; private set; }
  public int K { get; private set; }
  public CellColor Color { get; private set; }

  [SerializeField] protected GameObject PlacedObject;

  public void SetVar(int x, int z, int k, CellColor color) {
    X = x; Z = z; K = k; Color = color;
    HidePlacedObject();
  }

  public void ShowPlacedObject() {
    PlacedObject.SetActive(true);
  }

  protected void HidePlacedObject() {
    PlacedObject.SetActive(false);
  }
}