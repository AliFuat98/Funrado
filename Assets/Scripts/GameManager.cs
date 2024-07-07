using System.Xml.Schema;
using UnityEngine;

public class GameManager : MonoBehaviour {
  public static GameManager Instance { get; private set; }

  [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();
  [SerializeField] private float duration = 1f;
  private int totalMoveCount;
  private void Awake() {
    Instance = this;
  }

  public float GetDuration() {
    return duration;
  }

  void Start() {
    totalMoveCount = GridManager.Instance.GetCurrentLevelMoveCount();
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Mouse0)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask)) {
        var cell = raycastHit.transform.GetComponent<Cell>();
        cell.StartCollecting();
      }
    }
  }
}