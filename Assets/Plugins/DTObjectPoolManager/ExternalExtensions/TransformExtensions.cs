using System.Collections;
using UnityEngine;

namespace DTObjectPoolManager {
	public static class TransformExtensions {
		public static void RecycleAllChildren(this Transform transform, bool worldPositionStays = false) {
			GameObject[] children = new GameObject[transform.childCount];

			int index = 0;
			foreach (Transform child in transform) {
				children[index++] = child.gameObject;
			}

			for (int i = children.Length - 1; i >= 0; i--) {
				ObjectPoolManager.Recycle(children[i], worldPositionStays);
			}
		}
	}
}
