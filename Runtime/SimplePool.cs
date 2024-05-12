using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Almond
{
	public interface IPoolObj
	{
		string TemplateKey { get; }
		void Init(object[] param = null);
		void Release();
	}
	public abstract class PoolObj : MonoBehaviour, IPoolObj {
		public abstract string TemplateKey { get; }
		public abstract void Init(object[] param = null);
		public abstract void Release();
	}

	public class SimplePool
	{
		public static Dictionary<string, Queue<PoolObj>> poolDictionary = new Dictionary<string, Queue<PoolObj>>();
		private static Dictionary<string, PoolObj> templates = new Dictionary<string, PoolObj>();

		public void LoadAllPoolObjPrefabs() {
			var prefabs = Resources.LoadAll<PoolObj>("");
			foreach(var prefab in prefabs) {
				if(templates.ContainsKey(prefab.TemplateKey)) {
					Debug.LogWarning($"Template key \"{prefab.TemplateKey}\" is already contain");
					continue;
				}
				templates.Add(prefab.TemplateKey, prefab);
			}
		}

		public static T Instantiate<T>(T template, object[] param = null) where T : PoolObj {
			var key = template.TemplateKey;
			if(string.IsNullOrEmpty(key))
			{
				key = template.gameObject.name.ToString();
			}
			var pooledObj = GetPooledObject<T>(key, param);
			if(pooledObj == null)
			{
				templates.Add(key, template);
				pooledObj = Object.Instantiate(template);
				pooledObj.Init(param);
			}

			return pooledObj;
		}
		public static T Instantiate<T>(string key, object[] param = null) where T : PoolObj => GetPooledObject<T>(key, param);
		public static T InstantiatePrimitive<T>(PrimitiveType primitiveType, string key, object[] param = null) where T : PoolObj {
			var primitiveName = primitiveType.ToString() + key;
			var pooledObj = GetPooledObject<T>(primitiveName, param);
			if(pooledObj != null)
				return pooledObj;

			var gameObject = GameObject.CreatePrimitive(primitiveType);
			gameObject.name = primitiveName;
			var t = gameObject.AddComponent<T>();
			t.Init(param);
			return t;
		}

		private static T GetPooledObject<T>(string key, object[] param) where T : PoolObj
		{
			var hasPool = poolDictionary.TryGetValue(key, out var pool);
			if(hasPool)
			{
				if(pool.Count > 0)
				{
					var obj = (T)pool.Dequeue();
					obj.gameObject.SetActive(true);
					obj.Init(param);
					return obj;
				}
			}
			if(templates.TryGetValue(key, out var template)) {
				if(template is T t) {
					var obj = Object.Instantiate(t);
					obj.Init(param);
					return obj;
				}
				else {
					Debug.LogWarning($"Template type not match :: {key} :: type : {nameof(T)}");
					return null;
				}
			}
			else {
				Debug.LogWarning($"Template not found :: {key}");
				return null;
			}
		}

		public static void Release<T>(T obj, bool destroy = false) where T : PoolObj
		{
			var key = obj.TemplateKey;
			if(destroy)
			{
				Object.Destroy(obj.gameObject);
				return;
			}

			if(poolDictionary.TryGetValue(key, out var pool) == false)
			{
				Debug.LogWarning($"Create Pool :: Key :{key}");
				pool = new Queue<PoolObj>();
				poolDictionary.Add(key, pool);
			}
			pool.Enqueue(obj);
			obj.gameObject.SetActive(false);
		}

		public static void ReleaseOrDestroy<T>(T obj, bool destroy = false) where T : PoolObj
		{
			var key = obj.TemplateKey;
			if(destroy || poolDictionary.TryGetValue(key, out var pool) == false)
			{
				GameObject.Destroy(obj.gameObject);
				return;
			}

			pool.Enqueue(obj);
			obj.gameObject.SetActive(false);
		}

		public static void Clear(string key)
		{
			if(poolDictionary.TryGetValue(key, out var pool))
			{
				while(pool.Count > 0)
				{
					var obj = pool.Dequeue();
					GameObject.Destroy(obj.gameObject);
				}
				poolDictionary.Remove(key);
			}
		}
	}
}