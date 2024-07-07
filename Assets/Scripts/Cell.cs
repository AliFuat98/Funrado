using UnityEngine;

public class Cell : MonoBehaviour {
  public int X { get; private set; }
  public int Z { get; private set; }
  public int K { get; private set; }
  public CellColor CellColor { get; private set; }
  public Direction LookDirection { get; private set; }
  public Vector2Int LookDirectionVector { get; private set; }

  [SerializeField] protected GameObject PlacedObject;

  private bool isBusy = false;

  [SerializeField] protected GameObject BusyTestVisual;

  public void SetVar(int x, int z, int k, CellColor color, Direction direction) {
    X = x; Z = z; K = k; CellColor = color; LookDirection = direction;
    RotatePlacedObject();
    SetLookDirectionVector();
    HidePlacedObject();
  }

  public GameObject GetPlacedObject() {
    return PlacedObject;
  }

  public void ShowPlacedObject() {
    PlacedObject.SetActive(true);
  }

  protected void HidePlacedObject() {
    PlacedObject.SetActive(false);
  }

  public virtual void StartEating() {
  }

  public void SetBusy(bool busy) {
    isBusy = busy;

    // test
    var grid = GridManager.Instance.grid;
    var gridObject = grid.GetGridObject(X, Z);
    var topCell = gridObject.TopCell();

    BusyTestVisual.SetActive(true);

    if (topCell.K == K) {
      // this cell is the top cell show
      Renderer renderer = BusyTestVisual.GetComponent<Renderer>();
      if (busy) {
        // change the color red is busy
        renderer.material.color = Color.red;
      } else {
        // change the color green is not busy
        renderer.material.color = Color.green;
      }
    }
  }

  public bool IsCellBusy() {
    return isBusy;
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