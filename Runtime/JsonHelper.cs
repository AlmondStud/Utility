using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Almond {
	public static class JsonHelper {
		public static T[] FromJson<T>(string jsonFile) {
			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(jsonFile);
			return wrapper.items;
		}
		public static string ToJson<T>(T[] array) {
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.items = array;
			return JsonUtility.ToJson(wrapper);
		}
		public static string ToJson<T>(T[] array, bool prettyPrint) {
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.items = array;
			return JsonUtility.ToJson(wrapper, prettyPrint);
		}

		[Serializable]
		private class Wrapper<T> {
			public T[] items;
		}
	}
}