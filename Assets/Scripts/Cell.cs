using UnityEngine;

public class Cell : MonoBehaviour {
  public int X { get; private set; }
  public int Z { get; private set; }
  public int K { get; private set; }
  public CellColor Color { get; private set; }
  public Direction LookDirection { get; private set; }
  public Vector2Int LookDirectionVector { get; private set; }

  [SerializeField] protected GameObject PlacedObject;

  public void SetVar(int x, int z, int k, CellColor color, Direction direction) {
    X = x; Z = z; K = k; Color = color; LookDirection = direction;
    RotatePlacedObject();
    SetLookDirectionVector();
    HidePlacedObject();
  }

  public void ShowPlacedObject() {
    PlacedObject.SetActive(true);
  }

  protected void HidePlacedObject() {
    PlacedObject.SetActive(false);
  }

  public virtual void StartEating() {
  }

  private void RotatePlacedObject() {
    Vector3 direction = Vector3.zero;

    switch (LookDirection) {
      case Direction.Up:
        direction = Vector3.back;
        break;

      case Direction.Down:
        direction = Vector3.forward;
        break;

      case Direction.Left:
        direction = Vector3.right;
        break;

      case Direction.Right:
        direction = Vector3.left;
        break;
    }

    PlacedObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
  }

  private Vector2Int SetLookDirectionVector() {
    switch (LookDirection) {
      case Direction.Up:
        LookDirectionVector = new Vector2Int(0, 1);
        break;

      case Direction.Down:
        LookDirectionVector = new Vector2Int(0, -1);
        break;

      case Direction.Left:
        LookDirectionVector = new Vector2Int(-1, 0);
        break;

      case Direction.Right:
        LookDirectionVector = new Vector2Int(1, 0);
        break;
    }
    return default;
  }
}