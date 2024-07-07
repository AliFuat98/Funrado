using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {
  private LineRenderer lineRenderer;
  public List<Vector3> points { get; private set; }
  [SerializeField] private Tongue tongue;

  [SerializeField] private Transform startPoint;

  void Start() {
    lineRenderer = GetComponent<LineRenderer>();

    lineRenderer.widthMultiplier = 0.1f;
    lineRenderer.startColor = Color.red;
    lineRenderer.endColor = Color.red;

    points = new() {
      startPoint.position,
    };

    lineRenderer.positionCount = 1;
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
    float duration = 1.0f; // Çizginin tamamlanma süresi
    float elapsedTime = 0;

    while (elapsedTime < duration) {
      elapsedTime += Time.deltaTime;
      float t = elapsedTime / duration;
      Vector3 currentPoint = Vector3.Lerp(startPoint, endPoint, t);
      lineRenderer.SetPosition(points.Count - 2, startPoint);
      lineRenderer.SetPosition(points.Count - 1, currentPoint);

      // tongue settings
      tongue.transform.position = currentPoint;
      tongue.StartMovingForward();

      yield return null;
    }

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
      float duration = 1.0f; // line dissappear time
      float elapsedTime = 0;

      while (elapsedTime < duration) {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / duration;
        Vector3 currentPoint = Vector3.Lerp(endPoint, startPoint, t);
        lineRenderer.SetPosition(points.Count - 1, currentPoint);

        // tongue settings
        tongue.transform.position = currentPoint;
        tongue.StartMovingBackward();
        yield return null;
      }

      points.RemoveAt(points.Count - 1);

      // update line rendere
      lineRenderer.positionCount = points.Count;
      if (points.Count > 0) {
        lineRenderer.SetPositions(points.ToArray());
      }
    }
  }
}