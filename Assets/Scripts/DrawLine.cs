using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {
  private LineRenderer lineRenderer;
  public List<Vector3> points { get; private set; }
  [SerializeField] private Tongue tongue;

  [SerializeField] private Transform startPoint;
  private float duration;

  void Start() {
    lineRenderer = GetComponent<LineRenderer>();

    lineRenderer.widthMultiplier = 0.1f;
    lineRenderer.startColor = Color.red;
    lineRenderer.endColor = Color.red;

    points = new() {
      startPoint.position,
    };

    lineRenderer.positionCount = 1;

    duration = GameManager.Instance.GetDuration();
  }

  public void ClearTheLine() {
    points = new() {
      startPoint.position,
    };

    lineRenderer.positionCount = 1;
  }

  public void AddNewPointToDraw(Vector3 newPoint) {
    points.Add(newPoint);
    lineRenderer.positionCount = points.Count;
    StartCoroutine(DrawLineCoroutine());
  }

  private IEnumerator DrawLineCoroutine() {
    Vector3 startPoint = points[points.Count - 2];
    Vector3 endPoint = points[points.Count - 1];

    lineRenderer.SetPosition(points.Count - 2, startPoint);
    tongue.StartMovingForward();

    yield return AnimateLine(startPoint, endPoint);

    tongue.StopMoving();
    lineRenderer.SetPosition(points.Count - 1, endPoint);
  }

  public void UndoAllPoints() {
    StartCoroutine(RemoveAllPointCoroutine());
  }

  private IEnumerator RemoveAllPointCoroutine() {
    while (points.Count > 1) {
      Vector3 startPoint = points[points.Count - 2];
      Vector3 endPoint = points[points.Count - 1];
      tongue.StartMovingBackward();

      yield return AnimateLine(endPoint, startPoint);

      points.RemoveAt(points.Count - 1);

      // update line rendere
      lineRenderer.positionCount = points.Count;
      if (points.Count > 0) {
        lineRenderer.SetPositions(points.ToArray());
      }
    }
  }

  private IEnumerator AnimateLine(Vector3 startPoint, Vector3 endPoint) {
    float elapsedTime = 0;

    while (elapsedTime < duration) {
      elapsedTime += Time.deltaTime;
      float t = elapsedTime / duration;
      Vector3 currentPoint = Vector3.Lerp(startPoint, endPoint, t);

      lineRenderer.SetPosition(points.Count - 1, currentPoint);

      tongue.transform.position = currentPoint;

      yield return null;
    }
  }
}