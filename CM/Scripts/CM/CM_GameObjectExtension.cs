﻿using UnityEngine;

namespace CM
{
	public static class CM_GameObjectExtension
	{
		public static GameObject FindClosestGameObjectWithTag(this GameObject gameObject, string tag)
		{
			GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(tag);

			float closestDistance = -1;
			GameObject closestTarget = null;
			foreach (GameObject target in targetObjects)
			{
				float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);
				if (distance < closestDistance || closestDistance == -1)
				{
					closestDistance = distance;
					closestTarget = target;
				}
			}

			return closestTarget;
		}
	}
}