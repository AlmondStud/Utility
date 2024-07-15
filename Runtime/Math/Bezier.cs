using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Almond {
	/// <summary>
	/// Calculate bezier curve
	/// </summary>
	public static class Bezier {
		public static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2) {
			float u = 1 - t;
			float tt = t * t;
			float uu = u * u;
			float uuu = uu * u;

			Vector3 p = uu * p0;
			p += 2 * u * t * p1;
			p += tt * p2;

			return p;
		}
		public static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
			float u = 1 - t;
			float tt = t * t;
			float uu = u * u;
			float uuu = uu * u;
			float ttt = tt * t;

			Vector3 p = uuu * p0;
			p += 3 * uu * t * p1;
			p += 3 * u * tt * p2;
			p += ttt * p3;

			return p;
		}
		public static Vector3 CalculateBezierPoint(float t, List<Transform> points) {
			List<Vector3> _points = new List<Vector3>();
			foreach(Transform _t in points)
				_points.Add(_t.position);
			return CalculateBezierPoint(t, _points);
		}
		public static Vector3 CalculateBezierPoint(float t, List<Vector3> points) {
			if(points.Count == 3)
				return CalculateBezierPoint(t, points[0], points[1], points[2]);
			else if(points.Count == 4)
				return CalculateBezierPoint(t, points[0], points[1], points[2], points[3]);

			Queue<Vector3> pointQueue = new Queue<Vector3>(points);
			Queue<Vector3> tmpQueue = new Queue<Vector3>();
			while(pointQueue.Count > 1) {
				tmpQueue.Clear();
				Vector3 first = pointQueue.Dequeue();
				while(pointQueue.Count > 0) {
					Vector3 second = pointQueue.Dequeue();
					tmpQueue.Enqueue(Vector3.Lerp(first, second, t));
					first = second;
				}
				pointQueue = new Queue<Vector3>(tmpQueue);
			}

			return pointQueue.Dequeue();
		}

		public static List<Vector3> GetBezierLinePoints(List<Vector3> points, int splitCount) {
			var list = new List<Vector3>();
			for(int i = 0; i <= splitCount; i++) {
				list.Add(CalculateBezierPoint(((float)i / splitCount),points));
			}
			return list;
		}
	}
}