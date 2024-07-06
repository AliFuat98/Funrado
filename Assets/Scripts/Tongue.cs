using UnityEngine;

public class Tongue : MonoBehaviour {
  public LayerMask grapeLayerMask;

  private bool isMoving = false;

  public void StartMoving() {
    isMoving = true;
  }

  public void StopMoving() {
    isMoving = false;
  }

  void OnCollisionEnter(Collision collision) {
    Debug.Log("enter");
    if ((grapeLayerMask.value & (1 << collision.gameObject.layer)) != 0)
      Debug.Log(collision.transform.name);
  }

  private void OnTriggerEnter(Collider other) {
    if (!isMoving) {
      return;
    }
    Debug.Log("enter triggger");
    if ((grapeLayerMask.value & (1 << other.gameObject.layer)) != 0)
      Debug.Log(other.transform.name);
  }
}