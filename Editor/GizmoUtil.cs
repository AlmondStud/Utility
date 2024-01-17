using UnityEngine;

namespace Almond {
	public static class GizmoUtil {
		public static void DrawSolidCube(Vector3 pos, Vector3 size, Color color) {
			var half = size * 0.5f;
			var pos0 = pos;
			pos0 += half;

			var pos1 = pos;
			pos1 += half;
			pos1.z -= size.z;

			var pos2 = pos;
			pos2 -= half;
			pos2.y += size.y;

			var pos3 = pos;
			pos3 -= half;
			pos3.y += size.y;
			pos3.z += size.z;

			var pos4 = pos;
			pos4 += half;
			pos4.y -= size.y;

			var pos5 = pos;
			pos5 += half;
			pos5.z -= size.z;
			pos5.y -= size.y;

			var pos7 = pos;
			pos7 -= half;

			var pos8 = pos;
			pos8 -= half;
			pos8.z += size.z;

			//draw top
			var vertsT = new Vector3[] { pos0, pos1, pos2, pos3 };
			UnityEditor.Handles.DrawSolidRectangleWithOutline(vertsT, color, color * 0.9f);
			//draw bottom
			var vertsB = new Vector3[] { pos4, pos5, pos7, pos8 };
			UnityEditor.Handles.DrawSolidRectangleWithOutline(vertsB, color, color * 0.9f);
			//draw left
			var vertsL = new Vector3[] { pos0, pos4, pos5, pos1 };
			UnityEditor.Handles.DrawSolidRectangleWithOutline(vertsL, color, color * 0.9f);
			//draw right
			var vertsR = new Vector3[] { pos3, pos2, pos7, pos8 };
			UnityEditor.Handles.DrawSolidRectangleWithOutline(vertsR, color, color * 0.9f);
			//draw front
			var vertsF = new Vector3[] { pos3, pos0, pos8, pos4 };
			UnityEditor.Handles.DrawSolidRectangleWithOutline(vertsF, color, color * 0.9f);
			//draw back
			var vertsBack = new Vector3[] { pos2, pos1, pos5, pos7 };
			UnityEditor.Handles.DrawSolidRectangleWithOutline(vertsBack, color, color * 0.9f);
		}
		public static void DrawQuad(Vector3 pos, float size, Color color) {
			var half = 0.5f * size * Vector3.one;
			var pos0 = pos + half;
			pos0.y -= size;

			var pos1 = pos + half;
			pos1.z -= size;
			pos1.y -= size;

			var pos2 = pos - half;

			var pos3 = pos - half;
			pos3.z += size;

			var vertsB = new Vector3[] { pos0, pos1, pos2, pos3 };
			UnityEditor.Handles.DrawSolidRectangleWithOutline(vertsB, color, color * 0.9f);
		}
	}
}