using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

  [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();
  // Start is called before the first frame update
  void Start() {
  }

  // Update is called once per frame
  void Update() {
    if (Input.GetKeyDown(KeyCode.Mouse0)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask)) {
        var cell = raycastHit.transform.GetComponent<Cell>();
        GridManager.Instance.ClickCell(cell);
      }
    }
  }
}