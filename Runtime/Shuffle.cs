using System;
using System.Collections.Generic;
using System.Linq;

namespace Almond {
	public static class Shuffle {
		public static void ShuffleArray<T>(this T[] array, int count = -1) {
			if(array == null || array.Length <= 1)
				return;
			if(count == -1)
				count = array.Length;

			var tmp = array[0];
			for(int i = 0; i < count; i++) {
				var random = new Random().Next(array.Length - 1);
				array[0] = array[random];
				array[random] = tmp;
				tmp = array[0];
			}
		}

		public static List<T> ShuffleList<T>(this List<T> list, int count = 0) {
			var result = list.ToList();
			var tmp = result[0];
			for(int i = 0; i < count; i++) {
				var random = new Random().Next(result.Count - 1);
				result[0] = result[random];
				result[random] = tmp;
				tmp = result[0];
			}
			return result;
		}
	}
}