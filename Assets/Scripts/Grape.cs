using System.Collections;
using UnityEngine;

public class Grape : MonoBehaviour {
  [SerializeField] private LayerMask grapeLayerMask;
  [SerializeField] private LayerMask tongueLayerMask;
  private Cell[] cells;
  private float duration = 1.0f;
  private bool IsMoving = false;
  private CellFrog frog = null;
  private CellGrape cellGrape;
  private Animator animator;
  public bool lastGrape;

  private void Start() {
    duration = GameManager.Instance.GetDuration();
    animator = GetComponentInParent<Animator>();
    cellGrape = GetComponentInParent<CellGrape>();
  }

  private void DisableAnimator() {
    animator.enabled = false;
  }

  public void MoveToFrog(Cell[] cells) {
    DisableAnimator();
    cellGrape.UnparentPlacedObject();

    this.cells = cells;

    StartCoroutine(MoveAlongPoints());
  }

  private IEnumerator MoveAlongPoints() {
    IsMoving = true;
    if (lastGrape) {
      SoundManager.Instance.Reward();
    }
    for (int i = 0; i <= cells.Length - 1; i++) {
      Vector3 startPoint = transform.position;
      float offset = 0.2f;
      Vector3 endPoint = cells[i].transform.position + new Vector3(0, offset, 0);
      float elapsedTime = 0;

      Vector3 startScale = transform.localScale;
      Vector3 endScale = Vector3.zero;

      while (elapsedTime < duration) {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / duration;
        transform.position = Vector3.Lerp(startPoint, endPoint, t);

        // if it is the last one then shrink
        if (i == cells.Length - 1) {
          transform.localScale = Vector3.Lerp(startScale, endScale, t);
        }

        yield return null;
      }

      transform.position = endPoint; // Ensure the grape reaches the exact position
    }

    Destroy(gameObject);
  }

  private void OnTriggerEnter(Collider other) {
    if (IsMoving) {
      return;
    }

    // a grape touches this grape
    if ((grapeLayerMask.value & (1 << other.gameObject.layer)) != 0) {
      frog.MoveNextGrape();
    }

    // a tongue touches this grape (only voice and animations)
    if ((tongueLayerMask.value & (1 << other.gameObject.layer)) != 0) {
      var tongue = other.GetComponent<Tongue>();

      if (!tongue.IsMovingForward) {
        // do nothing
        return;
      }

      if (cellGrape.IsCellBusy()) {
        SoundManager.Instance.WrongMove();
        return;
      }

      if (cellGrape.CellColor == tongue.GetFrog().CellColor) {
        frog = tongue.GetFrog();
        animator.SetTrigger("CorrectTouched");
        SoundManager.Instance.CollectGrape(0);
      } else {
        animator.SetTrigger("WrongTouched");
        SoundManager.Instance.WrongMove();
      }
    }
  }
}