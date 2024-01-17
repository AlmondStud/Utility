using System;
using System.Collections.Generic;
using UnityEngine;

namespace Almond {
	public static class MathUtil {
		public static float Distance_XZ(Vector3 pos1, Vector3 pos2) {
			Vector3 tmp = pos1 - pos2;
			tmp.y = 0;
			return tmp.magnitude;
		}

		public static void CalcLayoutPositions(float[] positions, int count, float interval) {
			if(positions == null || positions.Length != count)
				return;

			var position = -interval;
			position *= (count % 2 == 0) ? ((count / 2) - 0.5f) : (count / 2);

			for(int i = 0; i < count; i++) {
				positions[i] = position;
				position += interval;
			}
		}

		public static List<float> CalcLayoutPositions(int count, float interval) {
			var positions = new List<float>();
			var position = -interval;
			position *= (count % 2 == 0) ? ((count / 2) - 0.5f) : (count / 2);

			for(var i = 0; i < count; i++) {
				positions.Add(position);
				position += interval;
			}

			return positions;
		}

		public static int GetLayoutIndex(float coord, int layoutCount, float interval) {
			var start = -interval * ((layoutCount % 2 == 0) ? ((layoutCount / 2) - 0.5f) : (layoutCount / 2));
			return Mathf.FloorToInt((coord - start) / interval);
		}
	}
}
