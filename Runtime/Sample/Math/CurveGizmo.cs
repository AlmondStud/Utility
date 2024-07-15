using Almond;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurveGizmo : MonoBehaviour
{
	[SerializeField] private Transform[] points;
	[Header("Point")]
	[SerializeField] private Color pointColor;
	[SerializeField, Range(0, 1)] private float shpereSize = 0.1f;
	[Header("Line")]
	[SerializeField] private Color bezierLineColor;
	[SerializeField] private Color splineLineColor;

	private void OnDrawGizmos() {
		if(points == null || points.Length < 2) {
			return;
		}
		var vaildPoints = points.Where(a => a != null).Select(a => a.position).ToList();
		if(vaildPoints.Count == 0)
			return;

		Gizmos.color = pointColor;
		foreach(var p in vaildPoints) {
			Gizmos.DrawSphere(p, shpereSize);
		}

		Gizmos.color = bezierLineColor;
		// Draw bezier
		var bezierPoints = Bezier.GetBezierLinePoints(vaildPoints, 100);
		for(int i = 0; i < bezierPoints.Count - 1; i++) {
			Gizmos.DrawLine(bezierPoints[i], bezierPoints[i + 1]);
		}
		// Draw cubic spline
		var vector2List = new List<Vector2>();
		foreach(var p in vaildPoints) {
			vector2List.Add(p);
		}
		Gizmos.color = splineLineColor;
		var cubicSplinePoints = CubicSpline.CalculateSpline(vector2List, 100);
		for(int i = 0; i < cubicSplinePoints.Count - 1; i++) {
			Gizmos.DrawLine(cubicSplinePoints[i], cubicSplinePoints[i + 1]);
		}
	}
}
