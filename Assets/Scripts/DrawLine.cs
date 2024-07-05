using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {
  private LineRenderer lineRenderer;
  private List<Vector3> points;

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
      yield return null;
    }

    lineRenderer.SetPosition(points.Count - 1, endPoint);
  }

  public void UndoLastPoint() {
    if (points.Count > 0) {
      // Son eklenen noktayý geri almak için coroutine baþlat
      StartCoroutine(RemoveLastPointCoroutine());
    }
  }

  private IEnumerator RemoveLastPointCoroutine() {
    Vector3 startPoint = points[points.Count - 2];
    Vector3 endPoint = points[points.Count - 1];
    float duration = 1.0f; // Çizginin kaybolma süresi
    float elapsedTime = 0;

    while (elapsedTime < duration) {
      elapsedTime += Time.deltaTime;
      float t = elapsedTime / duration;
      Vector3 currentPoint = Vector3.Lerp(endPoint, startPoint, t);
      lineRenderer.SetPosition(points.Count - 1, currentPoint);
      yield return null;
    }

    // Son noktayý listeden ve stack'ten çýkar
    points.RemoveAt(points.Count - 1);

    // Çizgiyi güncelle
    lineRenderer.positionCount = points.Count;
    if (points.Count > 0) {
      lineRenderer.SetPositions(points.ToArray());
    }
  }
}