using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Almond {
	public static class CubicSpline {
		private static int LineStepCount = 100;

		public static List<Vector2> CalculateSpline(List<Vector2> points, int splitCount = 100) => new CubicSplineData().FitAndEval(points, splitCount);		
		public class CubicSplineData {
			#region Fields

			// N-1 spline coefficients for N points
			private float[] a;
			private float[] b;

			private List<Vector2> pointOrigin;

			#endregion

			#region Private Methods
			private int _lastIndex = 0;
			private int GetNextXIndex(float x) {
				if(x < pointOrigin[_lastIndex].x) {
					throw new ArgumentException("The X values to evaluate must be sorted.");
				}

				while((_lastIndex < pointOrigin.Count - 2) && (x > pointOrigin[_lastIndex + 1].x)) {
					_lastIndex++;
				}

				return _lastIndex;
			}
			private float EvalSpline(float x, int j) {
				float dx = pointOrigin[j + 1].x - pointOrigin[j].x;
				float t = (x - pointOrigin[j].x) / dx;
				float y = (1 - t) * pointOrigin[j].y + t * pointOrigin[j + 1].y + t * (1 - t) * (a[j] * (1 - t) + b[j] * t); // equation 9
				return y;
			}
			#endregion

			#region Fit*
			public List<Vector2> FitAndEval(List<Vector2> points, int splitCount = 100, float startSlope = float.NaN, float endSlope = float.NaN) {
				Fit(points, startSlope, endSlope);
				return Eval(splitCount);
			}
			public void Fit(List<Vector2> points, float startSlope = float.NaN, float endSlope = float.NaN) {
				if(float.IsInfinity(startSlope) || float.IsInfinity(endSlope)) {
					throw new Exception("startSlope and endSlope cannot be infinity.");
				}
				pointOrigin = points;

				int n = points.Count;
				float[] r = new float[n]; // the right hand side numbers: wikipedia page overloads b

				TriDiagonalMatrixF m = new TriDiagonalMatrixF(n);
				Vector2 point1, point2;

				// First row is different (equation 16 from the article)
				if(float.IsNaN(startSlope)) {
					point1 = points[1] - points[0];
					m.C[0] = 1.0f / point1.x;
					m.B[0] = 2.0f * m.C[0];
					r[0] = 3 * (point1.y) / (point1.x * point1.x);
				}
				else {
					m.B[0] = 1;
					r[0] = startSlope;
				}

				// Body rows (equation 15 from the article)
				for(int i = 1; i < n - 1; i++) {
					point1 = points[i] - points[i - 1];
					point2 = points[i + 1] - points[i];

					m.A[i] = 1.0f / point1.x;
					m.C[i] = 1.0f / point2.x;
					m.B[i] = 2.0f * (m.A[i] + m.C[i]);

					r[i] = 3 * (point1.y / (point1.x * point1.x) + point2.y / (point2.x * point2.x));
				}

				// Last row also different (equation 17 from the article)
				if(float.IsNaN(endSlope)) {
					point1 = points[n - 1] - points[n - 2];
					m.A[n - 1] = 1.0f / point1.x;
					m.B[n - 1] = 2.0f * m.A[n - 1];
					r[n - 1] = 3 * (point1.y / (point1.x * point1.x));
				}
				else {
					m.B[n - 1] = 1;
					r[n - 1] = endSlope;
				}

				// k is the solution to the matrix
				float[] k = m.Solve(r);

				// a and b are each spline's coefficients
				this.a = new float[n - 1];
				this.b = new float[n - 1];

				for(int i = 1; i < n; i++) {
					point1 = points[i] - points[i - 1];
					a[i - 1] = k[i - 1] * point1.x - point1.y; // equation 10 from the article
					b[i - 1] = -k[i] * point1.x + point1.y; // equation 11 from the article
				}
			}
			#endregion

			#region Eval*
			public List<Vector2> Eval(int splitCount) {
				var returnPoints = new List<Vector2>();
				var stepSize = (pointOrigin[^1].x - pointOrigin[0].x) / (LineStepCount - 1);

				_lastIndex = 0;
				for(int i = 0; i < LineStepCount; i++) {
					var x = pointOrigin[0].x + (i * stepSize);
					int j = GetNextXIndex(x);
					var y = EvalSpline(x, j);
					returnPoints.Add(new Vector2(x, y));
				}
				return returnPoints;
			}
			#endregion
		}
		public class TriDiagonalMatrixF {
			/// <summary>
			/// The values for the sub-diagonal. A[0] is never used.
			/// </summary>
			public float[] A;

			/// <summary>
			/// The values for the main diagonal.
			/// </summary>
			public float[] B;

			/// <summary>
			/// The values for the super-diagonal. C[C.Length-1] is never used.
			/// </summary>
			public float[] C;

			/// <summary>
			/// The width and height of this matrix.
			/// </summary>
			public int N {
				get { return (A != null ? A.Length : 0); }
			}

			/// <summary>
			/// Indexer. Setter throws an exception if you try to set any not on the super, main, or sub diagonals.
			/// </summary>
			public float this[int row, int col] {
				get {
					int di = row - col;

					if(di == 0) {
						return B[row];
					}
					else if(di == -1) {
						Debug.Assert(row < N - 1);
						return C[row];
					}
					else if(di == 1) {
						Debug.Assert(row > 0);
						return A[row];
					}
					else
						return 0;
				}
				set {
					int di = row - col;

					if(di == 0) {
						B[row] = value;
					}
					else if(di == -1) {
						Debug.Assert(row < N - 1);
						C[row] = value;
					}
					else if(di == 1) {
						Debug.Assert(row > 0);
						A[row] = value;
					}
					else {
						throw new ArgumentException("Only the main, super, and sub diagonals can be set.");
					}
				}
			}

			/// <summary>
			/// Construct an NxN matrix.
			/// </summary>
			public TriDiagonalMatrixF(int n) {
				this.A = new float[n];
				this.B = new float[n];
				this.C = new float[n];
			}
			/// <summary>
			/// Solve the system of equations this*x=d given the specified d.
			/// </summary>
			/// <remarks>
			/// Uses the Thomas algorithm described in the wikipedia article: http://en.wikipedia.org/wiki/Tridiagonal_matrix_algorithm
			/// Not optimized. Not destructive.
			/// </remarks>
			/// <param name="d">Right side of the equation.</param>
			public float[] Solve(float[] d) {
				int n = this.N;

				if(d.Length != n) {
					throw new ArgumentException("The input d is not the same size as this matrix.");
				}

				// cPrime
				float[] cPrime = new float[n];
				cPrime[0] = C[0] / B[0];

				for(int i = 1; i < n; i++) {
					cPrime[i] = C[i] / (B[i] - cPrime[i - 1] * A[i]);
				}

				// dPrime
				float[] dPrime = new float[n];
				dPrime[0] = d[0] / B[0];

				for(int i = 1; i < n; i++) {
					dPrime[i] = (d[i] - dPrime[i - 1] * A[i]) / (B[i] - cPrime[i - 1] * A[i]);
				}

				// Back substitution
				float[] x = new float[n];
				x[n - 1] = dPrime[n - 1];

				for(int i = n - 2; i >= 0; i--) {
					x[i] = dPrime[i] - cPrime[i] * x[i + 1];
				}

				return x;
			}
		}
	}
}